# Rotterdam Quest

"Rotterdam Quest" is an augmented reality mobile game that takes players on an interactive tour of the city of Rotterdam. The game combines elements of exploration, puzzle-solving, and storytelling to create an engaging and educational experience.

## Gameplay

Players use their mobile devices to explore the city and interact with virtual objects and characters. The game features a map that guides players to different locations, where they can complete challenges and learn about the city's history and culture.

## Features

*   **Augmented Reality:** The game uses AR technology to overlay virtual objects and characters onto the real world.
*   **Interactive Storytelling:** The game features a branching narrative that allows players to make choices that affect the outcome of the story.
*   **Educational Content:** The game includes information about Rotterdam's history, architecture, and cultural heritage.
*   **Photo Mode:** Players can take photos of themselves with virtual characters and objects.

## Technical Details

The game is built using the Unity game engine and makes use of the following technologies:

*   **AR Foundation:** For handling augmented reality features.
*   **LeanTween:** For creating animations and visual effects.
*   **Native Gallery:** For saving photos to the device's gallery.

### Key Scripts

The project's key logic is organized into several key scripts:

*   **GameManager.cs**: A singleton that manages the overall game state, including player progress and scene transitions.
*   **ARPlacementManager.cs**: Handles the placement of AR objects in the real world, using AR Foundation's plane detection.
*   **QuestManager.cs**: Manages the quests, tracks objectives, and triggers story events.
*   **DialogueManager.cs**: Controls the display of dialogue and player choices during interactions with characters.
*   **MapController.cs**: Manages the in-game map, displaying points of interest and the player's location.
*   **PhotoCapture.cs**: Implements the photo mode functionality, allowing players to take pictures and save them to their device using Native Gallery.

## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

*   Unity Hub
*   Unity 2022.3.x or later

### Installation

1.  Clone the repo.
2.  Open the project in Unity Hub.
3.  Unity will download and install the necessary packages.
4.  Set the aspect ratio to 1080:2400 in your build settings. 
5.  Build for your target device (Android).

**NOTE: If you don't set the correct aspect ratio, the UI elements will appear stretched.**

