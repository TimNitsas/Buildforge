# Buildforge
A tool to manage video game builds.

- Uses Velopack to install desktop client updates
- A windows WPF client that consumes the API to retrieve, display, and manage builds
- Rest Api generated with NSwag with support for inheritance and versioning
- Uses an embedded WebView2 instance to securely authenticate users via GitHub OAuth
- The backend service issues JWTs to manage authenticated sessions and authorize API access
- The client application supports a custom URI scheme (buildforge://) for handling command-based interactions via an url
- The client application can remain minimized in the system tray