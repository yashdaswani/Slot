Project Overview
    Game Type: 3-reel slot machine with a single payline.
Features:
    5 vertically scrolling symbols per reel.
    Adjustable bet amount via a slider (5-50 credits).
    Win condition: All 3 reels match the middle symbol (index 2, y=100).
    Audio: UI clicks, spinning reels, win/lose sounds.
    Particle effects on win.
    Spin and reset buttons with interactability toggles.


Reel.cs:
    Manages individual reel behavior.
    Scrolls 6 images vertically (positions: 300, 200, 100, 0, -100, -200).
    Top image (y=300) is masked; bottom image (y=-300) resets to 300 with shuffled sprites.
SlotMachine.cs:
    Controls the overall game logic.
    Handles spinning, betting, win checking, UI updates, audio triggers, and particle effects.
    Reels spin sequentially with a 0.5s delay.
AudioManager.cs:
    Singleton audio manager with two AudioSources:
    uiSoundSource: For UI button clicks.
    gameplaySoundSource: For spinning, win, and lose sounds.


Webgl link - https://slot-assignment.netlify.app/

given below winning particle effect
![Screenshot 2025-03-20 113347](https://github.com/user-attachments/assets/8461e4a1-b051-461a-9b37-a24d523e2fd7)
