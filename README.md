# NetShop - E-Commerce Application

A modern, full-stack e-commerce platform for local store management with integrated payment processing.

## 🎯 Overview

NetShop is a fully functional e-commerce application that runs locally to manage a complete online store with Stripe payment integration.

## 🛠️ Tech Stack

- **.NET 10** - Latest framework version
- **Blazor Web** - Modern interactive UI
- **Minimal APIs** - High-performance REST endpoints
- **Stripe API** - Secure payment processing
- **Tailwind CSS** - Utility-first styling
- **Preline UI** - Premium component library

## ✨ Features

- 🛍️ Complete store management (products, inventory, orders)
- 💳 Stripe payment integration
- ⚡ Real-time interactive interface
- 📱 Responsive design with Tailwind CSS & Preline UI
- 🔐 Production-ready security

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js & npm
- Stripe account with API keys

### Installation

```bash
git clone https://github.com/nabashi404/NetShop.git
cd NetShop
npm install
dotnet build
dotnet run
```

### Configuration

Add your Stripe keys to `appsettings.json`:

```json
{
  "Stripe": {
    "PublishableKey": "your-publishable-key",
    "SecretKey": "your-secret-key"
  }
}
```

Access the application at `https://localhost:7001`

## 📦 Project Structure

```
NetShop/
├── Components/          # Blazor components
├── Services/           # Business logic
├── Pages/              # Blazor pages
├── Endpoints/          # Minimal API endpoints
├── appsettings.json    # Configuration
└── Program.cs          # API & app setup
```

## 🔧 Core Features

- **Minimal APIs** - Lightweight, high-performance endpoints
- **Blazor Web Components** - Interactive UI without JavaScript
- **Tailwind CSS & Preline UI** - Modern, responsive design system
- **Payment Processing** - Secure Stripe integration
- **Local Deployment** - Run on your own machine

## 📝 Status

Under Development - Portfolio Project
