Pushd %localappdata%\Android\android-sdk\platform-tools

adb kill-server
adb start-server

popd

pause