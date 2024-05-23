#!usr/bin/env bash

usage="
Usage: $(basename $0) [options] [command] [argument]

Commands:
  add              - Add Migration
  update           - Update Database
  remove           - Remove Migration
  drop             - Drop Database

Arguments:
  <name>           - Migration name (for add and update.)

Options:
  -c <DbContext>   - Specify the database context
  -h               - Display this help message
"
readonly STARTUP_PROJECT="src/Presentation/PublicApi"
readonly MIGRATION_PROJECT="src/Infrastructure/Migrator"

display_usage() { echo "$usage"; }

add_migration() {
    local name=$(echo "$1" | tr -d ' ')
    output=${context/DbContext}Migrations
    if [[ -z "$name" ]]; then echo "error:no specified target" >&2; exit 1; fi
    echo "Command: Add Migration '$name' ($context) ..."
    dotnet ef migrations add "$name" -p $MIGRATION_PROJECT -o $output -c $context -s $STARTUP_PROJECT
}

remove_migration() {
    echo "Command: Remove Migration ($context) ..."
    dotnet ef migrations remove -p $MIGRATION_PROJECT  -c $context -s $STARTUP_PROJECT
}

drop_database() {
    echo "Command: Drop Database ($context) ..."
    dotnet ef database drop -p $MIGRATION_PROJECT --c $context -s $STARTUP_PROJECT
}

update_database() {
    local target=$1
    echo "Command: Update Database ($context) ..."
    dotnet ef database update $target -c "$context" -s $STARTUP_PROJECT
}

#### ---- Main  program ---- ####
context=""

while getopts ":hc:" option; do
    case $option in
        h) display_usage; exit 0;;
        c) context=$(echo "$OPTARG" | tr -d ' ');;
        :) echo "error: '$OPTARG' requires an argument" >&2; exit 1;;
        \?) echo "error: Invalid option '$OPTARG'" >&2; exit 1;;
    esac
done

shift $((OPTIND - 1))

if [[ $# -lt 1 ]] 
then 
    echo "Error: No command or arguments provided." >&2 
    exit 1 
fi

command=$1
argument=$2

case "$command" in
    "add") add_migration $argument;;
    "drop") drop_database;;
    "remove") remove_migration;;
    "update") update_database $argument;;
    *) echo "error: Unknown command '$command'" >&2; exit 1;;
esac
