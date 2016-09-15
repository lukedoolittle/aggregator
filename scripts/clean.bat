Pushd ..\src

rmdir /s /q Material.UWP\bin
rmdir /s /q Material.UWP\obj
rmdir /s /q Material.Android\bin
rmdir /s /q Material.Android\obj
rmdir /s /q Material.iOS\bin
rmdir /s /q Material.iOS\obj
rmdir /s /q Material.Windows\bin
rmdir /s /q Material.Windows\obj
rmdir /s /q Material\bin
rmdir /s /q Material\obj

rmdir /s /q Foundations\bin
rmdir /s /q Foundations\obj
rmdir /s /q Foundations.Cryptography\bin
rmdir /s /q Foundations.Cryptography\obj
rmdir /s /q Foundations.Http.Windows\bin
rmdir /s /q Foundations.Http.Windows\obj
rmdir /s /q Foundations.HttpClient\bin
rmdir /s /q Foundations.HttpClient\obj
rmdir /s /q Foundations.Serialization\bin
rmdir /s /q Foundations.Serialization\obj

popd
Pushd ..\test

rmdir /s /q Foundations.Test\bin
rmdir /s /q Foundations.Test\obj
rmdir /s /q Quantfabric.Test.Android\bin
rmdir /s /q Quantfabric.Test.Android\obj
rmdir /s /q Quantfabric.Test.iOS\bin
rmdir /s /q Quantfabric.Test.iOS\obj
rmdir /s /q Quantfabric.Test.Windows\bin
rmdir /s /q Quantfabric.Test.Windows\obj
rmdir /s /q Quantfabric.UI.Test.Android\bin
rmdir /s /q Quantfabric.UI.Test.Android\obj
rmdir /s /q Quantfabric.UI.Test.iOS\bin
rmdir /s /q Quantfabric.UI.Test.iOS\obj
rmdir /s /q Quantfabric.UI.Test.UWP\bin
rmdir /s /q Quantfabric.UI.Test.UWP\obj
rmdir /s /q Quantfabric.UI.Test.UWP\AppPackages
rmdir /s /q Quantfabric.Web.Test\bin
rmdir /s /q Quantfabric.Web.Test\obj

popd

rmdir /s /q ..\packages

pause