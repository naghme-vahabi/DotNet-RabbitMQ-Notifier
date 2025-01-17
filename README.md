# Message Broker Implementation with RabbitMQ

## 🎯Overview
This project demonstrates a robust implementation of a message broker system using RabbitMQ, designed with clean architecture principles. It supports multiple message types (SMS and Email) with separate handling mechanisms, showcasing enterprise-level messaging patterns and best practices.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/8.0)

## ✨ Key Features
- 🔄 Asynchronous message processing
- 📨 Support for multiple message types (SMS & Email)
- 🏗️ Clean Architecture implementation
- 🔌 Dependency Injection
- 📝 Comprehensive logging
- ⚡ High-performance message handling
- 🔍 Message validation and error handling
- 🛠️ Configurable settings

## Architecture
The project follows Clean Architecture principles with distinct layers:
- **Domain Layer**: Core business logic and interfaces
- **Application Layer**: Application services and DTOs
- **Infrastructure Layer**: External service implementations
- **API/Presentation Layer**: API endpoints and controllers

## Prerequisites
- .NET 6.0+
- RabbitMQ Server
- SMTP Server (for email functionality)
- SMS Provider API access

## Getting Started

### 1. Installation
```bash
git clone https://github.com/yourusername/rabbitmq-message-broker.git
cd rabbitmq-message-broker
dotnet restore
```


### 2. Configuration
Update appsettings.json with your configurations:
```bash
{
  "RabbitMQSetting": {
    "ExchangeName": "message_exchange",
    "QueueName": "message_queue",
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.yourserver.com",
    "Port": 587,
    "Username": "your-email",
    "Password": "your-password"
  },
  "SMSSettings": {
    "ApiEndpoint": "https://api.smsprovider.com",
    "ApiKey": "your-api-key"
  }
}
```
### 3. 🏃‍♀️Running the Application
```bash
    dotnet build
    dotnet run
```
### 4. 📣Message Flow

Messages are published to RabbitMQ exchange
Messages are routed based on their type
Consumers process messages asynchronously
Messages are delivered via appropriate service (Email/SMS)
Acknowledgment/rejection handling ensures message delivery

### 5. Error Handling

--Comprehensive exception handling
--Message retry mechanism
--Dead letter queue support
--Detailed logging

### 6. Project Structure
├── src/ </br>
│   ├── Domain/</br>
│   │   ├── Entities/</br>
│   │   ├── Interfaces/</br>
│   │   └── Exceptions/</br>
│   ├── Common/</br>
│   ├── Application/</br>
│   │   ├── Services/</br>
│   ├── Infrastructure/</br>
│   │   ├── MessageBroker/</br>
│   │   └── Services/</br>
│   └── API/</br>


### 7. Contributing

Fork the project</br>
Create your feature branch (git checkout -b feature/AmazingFeature)</br>
Commit your changes (git commit -m 'Add some AmazingFeature')</br>
Push to the branch (git push origin feature/AmazingFeature)</br>
Open a Pull Request</br>

### 8. Best Practices Demonstrated

Message Queue Patterns</br>
Dependency Injection</br>
Repository Pattern</br>
SOLID Principles</br>
Error Handling</br>
Logging</br>
Configuration Management</br>
