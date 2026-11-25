# DECISIONS.md — Juicy Journey (Assignment 10)

## Week 11

**2026-11-10 – 2026-11-11**

I made three enemies: Patroller, Shooter and Charger. Each one has its own states. The Patroller goes Patrol, Chase, Attack. The Shooter goes Idle, Aim, Shoot, Cooldown. The Charger goes Idle, Wind-up, Charge, Recover. All three are in Level 1, and I also put one Patroller in Level 2.

I asked the AI to explain the assignment and help me fix errors. It made a mistake with the Patroller: it didn’t walk in one specific area, it just wandered around. I fixed it myself. I put two points, WP_A and WP_B, as borders, and now the Patroller walks only between them.

Today I fixed some bugs with enemy detection, damage, and movement near the edges of platforms.

Then I did the playtest on a phone with two people who had never seen my game. Zamira beat the level in 2 tries. Nurakhmet needed 4 or 5 tries. That was a bit too hard for the second player.

AI helped me understand the assignment and fix some errors, but I wrote and tested the enemy logic myself.

## Week 12

**2026-11-18**  Started assignment 11: plan to move balance numbers into ScriptableObjects and add JSON save.
**2026-11-18 ** I asked the AI agent to explain the assignment and tell me what I needed to do. I created a Data folder inside Scripts and added these scripts: BossStats, ChargerStats, EnemyStats, ItemData, LevelData, PatrollerStats, PlayerStats, and ShooterStats. I also created a Data folder inside Assets for the assets. The agent showed me how to create the stats assets, but some of them were not correct for my game, so I changed them. I also fixed an old problem where the boss health UI was not decreasing on the screen.

**2026-11-18** I added ItemData for Watermelon and Kiwi. I did not need to create new prefabs because they were already prefabs. Some errors appeared, and the agent helped me find and fix them.

**2026-11-18 **I created a separate Game scene and made Level_01, Level_02, and Level_03 as LevelData assets. The Game scene uses these assets for the levels.

**2026-11-18 **After I defeated the boss, the winScreen did not appear, even though there were no errors in the Console. At first, I thought the problem was with the victory logic or Build Profiles. Later, I found out that the problem was with the UI because the parent object was disabled. I enabled the UI again and left the winScreen, Game Over Screen, and PauseScreen disabled. They will now be enabled only by the code when needed.

**2026-11-18 **Trap.cs showed the message “Player Spawn Point is not assigned” and the player was not teleported. The problem was that prefabs cannot keep references to objects from a scene. I added the Respawn tag to the PlayerSpawnPoint and used FindWithTag in Awake to find it.

**2026-11-18** I set the company and product names in Project Settings. The company name is Dinara, and the product name is Juicy Journey.

**2026-11-18** I asked the AI how I could create a JSON file and asked it to explain it to me. The AI explained what a JSON file is, how to create one, and how it can be used in my project.

**2026-11-19 **The agent first gave me instructions for making an Android build using Build APK. However, I have an iPhone and a Mac, so this option did not work for me. I changed the build to iOS, opened the project in Xcode, and installed the game on my iPhone using my Apple ID. After that, I recorded the demo video directly from my phone instead of using an APK.

**2026-11-19** The agent showed me that the Editor menu (Tools > Save) only works in the Unity Editor and does not work on the phone. I tested the save.json problem in the Unity Editor and then checked on my phone if the progress was saved after closing the app. The progress was saved successfully.

## Week 13

**2026-11-25** I added sounds to my game. First, I downloaded all the sounds from Pixabay.com and added them to Unity. I asked AI to explain the assignment to me and tell me where I could download good sounds.

Then I created two scripts: **MusicManager** and **VolumeSettings**, and changed the code in **SoundManager.cs**. After that, I created **MainMixer** and asked AI how to set it up.

Then I added all the sounds to the **SoundManager** that I have in the Hierarchy. At first, the **UISound** did not work. Then I understood the logic, and for each UI button, I added **SoundManager** to the OnClick event and selected **UISound**.

The same thing happened with the enemies. They did not make a sound when the player came close to them, so I added a few lines of code to the enemy scripts.

After that, I added **UI Settings** and a **SettingsPanel**, where I can change the volume of the music and SFX.

I checked everything to make sure all the sounds worked well and could be heard clearly. Then I saved the project and uploaded it to my phone. I have an iPhone, so I did this using Xcode. Finally, I made a commit and created a new branch called Assignment 12. I also asked AI how to make the SettingsPanel, but at first it gave me the wrong solution because it did not know my project well enough. Because of this, there were many confusing and unnecessary settings. So I simplified the SettingsPanel myself, so everything is easy to understand and the user can immediately see what to do. I also made it match the style of my game, so it fits Juicy Journey.
