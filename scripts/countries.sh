#!usr/bin/env bash

# error handling
set -euo pipefail

readonly REQ_URL="https://countriesnow.space/api/v0.1/countries/info?returns=iso2,iso3,currency,dialCode,flag"
readonly OUTPUT_DIR="static/json"
readonly OUTPUT_FILE="$OUTPUT_DIR/countries.json"

mkdir -p $OUTPUT_DIR

log_info() { echo "[INFO] $1">&2; }
log_error() { echo "[ERROR] $1">&2; }

fetch_data() {
    local response
    if ! response=$(curl -sf --location "$REQ_URL"); then
        log_error "Failed to fetch data for request '$REQ_URL'"
        return 1
    fi

    echo "$response"
}

process_data() {
    local json=$1
    local data=()

    while IFS= read -r row; do

        local name
        local cca2
        local cca3
        local currency

        name=$(echo ${row} | jq -r '.name')
        cca2=$(echo ${row} | jq -r '.iso2')
        cca3=$(echo ${row} | jq -r '.iso3')
        currency=$(echo ${row} | jq -r '.currency')

        [[ -z "$name" || -z "$cca2" || -z "$cca3" ]] && continue

        data+=("{
            \"name\": \"$name\",
            \"cca2\": \"$cca2\",
            \"cca3\": \"$cca3\",
            \"currency\": \"$currency\"
        }")
    done < <(echo "${json}" | jq -c '.data[]')

    local data_string
    data_string=$(printf "%s," "${data[@]}")
    data_string=${data_string%,}

    echo "[${data_string}]"
}

log_info "Fetching data..."
response=$(fetch_data)
process_data "$response" > $OUTPUT_FILE
log_info "Data saved to ${OUTPUT_FILE}"
