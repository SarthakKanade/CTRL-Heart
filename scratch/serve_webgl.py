import http.server
import socketserver
import os
import sys

PORT = 8080
DIRECTORY = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "CTRL - HEART_Web_Build"))

class UnityWebHandler(http.server.SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=DIRECTORY, **kwargs)

    def end_headers(self):
        # Enable CORS and Unity WebGL multi-threading / isolation headers
        self.send_header('Access-Control-Allow-Origin', '*')
        self.send_header('Cross-Origin-Opener-Policy', 'same-origin')
        self.send_header('Cross-Origin-Embedder-Policy', 'require-corp')
        
        # Add Brotli content-encoding for compressed Unity assets
        if self.path.endswith('.br'):
            self.send_header('Content-Encoding', 'br')
            if '.wasm' in self.path:
                self.send_header('Content-Type', 'application/wasm')
            elif '.js' in self.path:
                self.send_header('Content-Type', 'application/javascript')
            elif '.data' in self.path:
                self.send_header('Content-Type', 'application/octet-stream')

        # Add Gzip content-encoding if gz assets are present
        elif self.path.endswith('.gz'):
            self.send_header('Content-Encoding', 'gzip')
            if '.wasm' in self.path:
                self.send_header('Content-Type', 'application/wasm')
            elif '.js' in self.path:
                self.send_header('Content-Type', 'application/javascript')
            elif '.data' in self.path:
                self.send_header('Content-Type', 'application/octet-stream')

        super().end_headers()

if __name__ == '__main__':
    # Allow port reuse
    socketserver.TCPServer.allow_reuse_address = True
    with socketserver.TCPServer(("", PORT), UnityWebHandler) as httpd:
        print(f"Serving Unity WebGL from {DIRECTORY} at http://localhost:{PORT}")
        sys.stdout.flush()
        httpd.serve_forever()
