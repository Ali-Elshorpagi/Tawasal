
# Tawasal

## Overview

Tawasal is a social media app built using **ASP.NET MVC**. The app allows users to create profiles, connect with friends, share posts, and interact with content through likes and comments. The platform provides notifications to enhance user engagement and keep everyone connected.

## Features

- **User Profiles**:  
  Users can create, view, and update their profiles with personal information, including profile pictures, bios, and more.

- **Posting & Timeline**:  
  Users can create posts and view posts from their friends and followings on their timeline.

- **Engagement (Likes & Comments)**:  
  Users can like and comment on posts from their friends and followings.

- **Social Connections**:  
  Users can send friend requests, follow other users, and manage their connections.

- **Notifications**:  
  - New comments on posts
  - Likes on posts
  - Friend requests
  - New followers

## Technologies Used

- **ASP.NET MVC**
- **Entity Framework Core** (Database ORM)
- **SQL Server** (Database)
- **HTML/CSS/JavaScript** (Frontend)
- **Bootstrap** (UI Framework)

## Installation & Setup

1. Clone the repository:
    ```bash
    git clone https://github.com/Ali-Elshorpagi/Tawasal.git
    ```

2. Navigate to the project directory:
    ```bash
    cd Tawasal
    ```

3. Set up the database connection in `appsettings.json`.

4. Apply migrations:
    ```bash
    dotnet ef database update --project Tawasal
    ```

5. Run the application:
    ```bash
    dotnet run --project Tawasal
    ```

6. Access the application in your browser at `http://localhost:5037`

## Contributing

Contributions are welcome! Feel free to submit issues or pull requests.
