#!/usr/bin/env bash

# Constants
readonly STARTUP_PROJECT="src/Presentation/PublicApi"
readonly MIGRATION_PROJECT="src/Infrastructure/Migrator"

usage="
Usage: $(basename $0) [options] [command] [argument]

Commands:
  add     <name>      Add a new migration with specified name
  update  [target]    Update database to specified migration (or latest)
  remove              Remove the last migration
  drop                Drop the entire database
  list                List all migrations

Options:
  -c     <DbContext>  Specify the database context (default: StoreDbContext)
  -h                  Display this help message

Examples:
  $(basename $0) -c StoreDbContext add InitialCreate
  $(basename $0) -c StoreDbContext update
  $(basename $0) update InitialCreate
"

# Functions
display_usage() { 
    echo "$usage" 
}

check_dotnet_ef() {
    if ! command -v dotnet ef &> /dev/null; then
        echo "Error: dotnet-ef tool is not installed" >&2
        exit 1
    fi
}

add_migration() {
    local name=$(echo "$1" | tr -d ' ')
    local output=${context/DbContext}Migrations

    if [[ -z "$name" ]]; then 
        echo "Error: Migration name is required" >&2
        exit 1
    fi

    echo "Adding migration '$name' for context '$context'..."
    dotnet ef migrations add "$name" \
        -p "$MIGRATION_PROJECT" \
        -o "$output" \
        -c "$context" \
        -s "$STARTUP_PROJECT" || exit 1
}

remove_migration() {
    echo "Removing last migration for context '$context'..."
    dotnet ef migrations remove \
        -p "$MIGRATION_PROJECT" \
        -c "$context" \
        -s "$STARTUP_PROJECT" || exit 1
}

drop_database() {
    echo "Dropping database for context '$context'..."
    dotnet ef database drop \
        -p "$MIGRATION_PROJECT" \
        -c "$context" \
        -s "$STARTUP_PROJECT" || exit 1
}

update_database() {
    local target=$1
    local target_msg=${target:-"latest migration"}
    
    echo "Updating database to $target_msg for context '$context'..."
    dotnet ef database update "$target" \
        -c "$context" \
        -s "$STARTUP_PROJECT" || exit 1
}

list_migrations() {
    echo "Listing migrations for context '$context'..."
    dotnet ef migrations list \
        -p "$MIGRATION_PROJECT" \
        -c "$context" \
        -s "$STARTUP_PROJECT" || exit 1
}

# --- Main --- #

check_dotnet_ef

context=""

# Parse options
while getopts ":hc:" option; do
    case $option in
        h) display_usage; exit 0 ;;
        c) context=$(echo "$OPTARG" | tr -d ' ') ;;
        :) echo "Error: -$OPTARG requires an argument" >&2; exit 1 ;;
        \?) echo "Error: Invalid option -$OPTARG" >&2; exit 1 ;;
    esac
done

shift $((OPTIND - 1))

# Validate input
if [[ $# -lt 1 ]]; then 
    echo "Error: Command required." >&2
    display_usage
    exit 1
fi

# Execute command
command=$1
argument=$2

case "$command" in
    "add") add_migration "$argument" ;;
    "update") update_database "$argument" ;;
    "remove") remove_migration ;;
    "drop") drop_database ;;
    "list") list_migrations ;;
    *) echo "Error: Unknown command '$command'" >&2; exit 1 ;;
esac
