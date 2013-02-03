#!/bin/bash -x
cd "$(dirname "$0")"
export DYLD_FALLBACK_LIBRARY_PATH="/Library/Frameworks/Mono.framework/Versions/Current/lib:/lib:/usr/lib"
exec mono "Desktop Ponies.exe" "$@"
