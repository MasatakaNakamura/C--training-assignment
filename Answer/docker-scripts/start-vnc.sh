#!/bin/bash

# VNC起動スクリプト
echo "Starting VNC server for Customer Management Application..."

# X仮想フレームバッファを起動
Xvfb :1 -screen 0 1024x768x16 &
export DISPLAY=:1

# ウィンドウマネージャを起動
fluxbox &

# VNCサーバーを起動
x11vnc -display :1 -nopw -listen localhost -xkb -ncache 10 -ncache_cr -forever -shared &

# VNCパスワードが設定されている場合
if [ ! -z "$VNC_PASSWORD" ]; then
    mkdir -p ~/.vnc
    echo "$VNC_PASSWORD" | vncpasswd -f > ~/.vnc/passwd
    chmod 600 ~/.vnc/passwd
    x11vnc -display :1 -rfbauth ~/.vnc/passwd -listen localhost -xkb -ncache 10 -ncache_cr -forever -shared &
fi

# 少し待機してからアプリケーション起動
sleep 3

echo "Starting Customer Management Application..."

# .NETアプリケーションを起動
cd /app/publish

# 環境変数でDB接続文字列を設定
export ConnectionStrings__DefaultConnection="$CONNECTION_STRING"

# Windows Formsアプリケーションの実行を試行
# 注意: Linux環境でのWindows Forms実行は制限があります
echo "Attempting to run .NET application..."

# .NET 6でのWindows Forms実行を試行（通常は失敗する）
if dotnet CustomerManager.WinForms.dll 2>/dev/null; then
    echo "Application started successfully"
else
    echo "Windows Forms application cannot run in Linux container"
    echo "Alternative approaches:"
    echo "1. Use Windows containers"
    echo "2. Port to ASP.NET Core Blazor"
    echo "3. Create console version"
    echo "4. Use database-only Docker setup"
    
    # 代替: 簡単なWebサーバーを起動してメッセージを表示
    echo "<html><body><h1>Customer Management System</h1><p>Windows Forms app requires local execution or web migration.</p></body></html>" > /tmp/index.html
    cd /tmp && python3 -m http.server 8000 &
fi

# フォアグラウンドプロセスを維持
wait