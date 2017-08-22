#!/bin/bash -x
cd "$(dirname "$0")"
export DYLD_FALLBACK_LIBRARY_PATH="/Library/Frameworks/Mono.framework/Versions/Current/lib:/lib:/usr/lib"

if hash mono32 2>/dev/null; then
   exec mono32 "Desktop Ponies.exe" "$@"
else
   exec mono "Desktop Ponies.exe" "$@"
fi
