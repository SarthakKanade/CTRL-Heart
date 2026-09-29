#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PORT=8080

# Check if server is already running on port 8080
if ! lsof -i :$PORT > /dev/null 2>&1; then
    python3 "$DIR/scratch/serve_webgl.py" > /dev/null 2>&1 &
    sleep 0.5
fi

open "http://localhost:$PORT"
