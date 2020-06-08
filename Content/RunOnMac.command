#!/bin/bash
cd "$(dirname "$0")"

tf=(true false)

# check what executables we have for mono
hash mono32 2>/dev/null; have_mono32=${tf[$?]} 
hash mono 2>/dev/null; have_mono=${tf[$?]}
if ! $have_mono32 && ! $have_mono; then
   echo "You need to install Mono. Go to:"
   echo "https://www.mono-project.com/download/"
   exit 1
fi

# check macOS version to use the appropriate launcher
os_version=$(sw_vers -productVersion | tr '.' ' ')
version_parts=($os_version)
[ ${version_parts[0]} -gt 10 ] || [ ${version_parts[0]} -eq 10 ] && [ ${version_parts[1]} -ge 15 ]
catalina_or_later=${tf[$?]}
sw_vers  # output macOS version to encourage including it while reporting bugs

# make sure libraries can be found
export DYLD_FALLBACK_LIBRARY_PATH="/Library/Frameworks/Mono.framework/Versions/Current/lib:/lib:/usr/lib"

# launch!
if $catalina_or_later && $have_mono; then
    exec mono "Desktop Ponies.exe" "$@"
elif $have_mono32; then
    exec mono32 "Desktop Ponies.exe" "$@"
elif $have_mono; then
    exec mono "Desktop Ponies.exe" "$@"
else
    echo 'Launch failed!'
    exit 1
fi
