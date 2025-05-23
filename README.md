# FairSplit

## Background and Motivation

FairSplit is a mobile application designed to simplify the process of splitting expenses among groups. Whether it's a group dinner, a weekend trip, or shared monthly bills, calculating who owes what can be time-consuming and confusing. FairSplit was developed to automate and streamline this process, ensuring fair and accurate calculations for all participants.

The motivation behind FairSplit stems from real-life challenges faced in group settings where shared expenses need to be tracked. Often, individuals rely on manual notes, group chats, or basic calculators to keep track of spending, which can lead to errors and miscommunication. This app addresses those issues by providing a centralized, user-friendly, and cloud-enabled solution that handles bill splitting transparently and securely.

This project was developed as part of the 6002CEM Mobile App Development module at Coventry University, showcasing key skills such as .NET MAUI development, MVVM architecture, cloud integration, and mobile UI design.

## Features Demonstrated in the Video

The video demonstration (linked separately) will showcase the following key features of the application:

### 1. **Login and Registration (Firebase Authentication)**
Users can register and log in securely using their email and password. This is implemented via Firebase Authentication, and the video will show both the registration and login process. Input validation and error handling are included.

### 2. **Home Page**
After logging in, users are directed to the Home screen, which acts as the central navigation point for the app. The video will show the UI elements and navigation controls available here.

### 3. **Add Participants**
Users can add participants who are involved in a shared expense. The video will demonstrate how a user can enter participant names, assign them to groups, and prepare them for expense assignment.

### 4. **Create an Expense**
Users can add a new expense, assign participants to that expense, and input the total amount. The app will then automatically calculate and assign each person’s share. The video will demonstrate this process in detail.

### 5. **Summary Page**
The summary screen shows a clear breakdown of who owes what to whom, based on the expenses added. The demo will walk through a completed example where a group of participants shares multiple expenses and see how the final debts are calculated and displayed.

### 6. **Profile Management**
Users can view and update their profile information. The video will demonstrate navigation to the profile screen and updating basic user info.

### 7. **Settings Page**
The video will show how users can access and modify application settings or log out of their account.

## Technical Overview

FairSplit is built using the .NET MAUI framework with C# and follows the MVVM (Model-View-ViewModel) architectural pattern. This separation of logic and UI improves maintainability and scalability. The application uses Firebase for both authentication and real-time data storage, making the app functional across multiple platforms with synced data.

The UI is built using XAML with .NET MAUI controls such as `CollectionView`, `StackLayout`, `Entry`, and `Button`, and includes responsive layouts across screens. Navigation is managed using .NET MAUI Shell routing.

Each View (screen) is backed by its corresponding ViewModel, ensuring that business logic is cleanly separated from UI elements.

## Final Notes

FairSplit provides a practical and efficient solution for real-world bill splitting problems using modern mobile development tools and cloud technologies. The video will clearly demonstrate each functional part of the app from login to expense summary.

All code is available in this repository and documented with comments for clarity.
