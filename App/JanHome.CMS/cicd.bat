cd %windir%\system32\inetsrv
Start appcmd.exe
appcmd stop site /site.name:DiDongxanh
cd C:\inetpub\wwwroot\DiDongXanh\CMS
git add .
git commit -am "commit release"
git pull origin release
call npm install
call npm run build
dotnet publish
cd %windir%\system32\inetsrv
appcmd start site /site.name:DiDongxanh
