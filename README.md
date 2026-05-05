# Buildforge

A tool to manage video game builds.

## Features

- Uses Velopack to install desktop client updates  
- A Windows WPF client that consumes the API to retrieve, display, and manage builds  
- REST API generated with NSwag with support for inheritance and versioning  
- Uses an embedded WebView2 instance to securely authenticate users via GitHub OAuth  
- The backend service issues JWTs to manage authenticated sessions and authorize API access  
- The client application supports a custom URI scheme (`buildforge://`) for handling command-based interactions via a URL  
- The client application can remain minimized in the system tray  
- The backend service has a strong focus on versioning to provide an always-on service with no downtime for migration  

## Screenshots

> Build View

![Build](https://raw.githubusercontent.com/TimNitsas/Buildforge/refs/heads/main/Buildforge.Docs/Image.Build.jpg)
