# Clicker Weather Breeds App

A small Unity application created as a technical assignment.

The project demonstrates:
- UI architecture
- Dependency Injection (Zenject)
- Request Queue
- HTTP communication
- Request cancellation
- ScriptableObject driven configuration
- Modular feature architecture

---

## Features

### 👇 Clicker

- Tap button to earn coins
- Auto click every 3 seconds
- Energy system
- Configurable values via ScriptableObjects
- Click VFX
- Coin animation
- Sound effects

---

### ☁️ Weather

Uses the National Weather Service API.

Features:

- Automatic weather updates every 5 seconds
- Request Queue
- Automatic request cancellation when leaving the page
- Weather icon and temperature display

---

### 🐶 Dog Breeds

Uses Dog API.

Features:

- Load dog breeds
- Loading indicator
- Breed details popup
- Automatic cancellation of obsolete requests
- Adaptive popup height

---

## Architecture

The project follows a modular architecture.

- MVP approach
- Dependency Injection via Zenject
- Feature-oriented structure
- Request Queue for all HTTP operations
- CancellationToken based request cancellation

---

## Technologies

- Unity 2022.3+
- C#
- Zenject
- UnityWebRequest
- UniTask
- UniRx

---

## APIs

Weather API

https://api.weather.gov/

Dog API

https://dogapi.dog/


## Project Structure

```
Assets
 ├── Core
 ├── Infrastructure
 ├── Services
 ├── Features
 │    ├── Clicker
 │    ├── Weather
 │    └── DogBreeds
 └── UI
```

---

## Main Concepts

- Request Queue
- Request Cancellation
- Dependency Injection
- Feature Separation
- Configurable Gameplay
- Reusable UI
- ScriptableObject Configuration

---
