# DECISIONS.md — Juicy Journey (Assignment 10)

## Week 11

**2026-11-10 – 2026-11-11**

I made three enemies: Patroller, Shooter and Charger. Each one has its own states. The Patroller goes Patrol, Chase, Attack. The Shooter goes Idle, Aim, Shoot, Cooldown. The Charger goes Idle, Wind-up, Charge, Recover. All three are in Level 1, and I also put one Patroller in Level 2.

I asked the AI to explain the assignment and help me fix errors. It made a mistake with the Patroller: it didn’t walk in one specific area, it just wandered around. I fixed it myself. I put two points, WP_A and WP_B, as borders, and now the Patroller walks only between them.

Today I fixed some bugs with enemy detection, damage, and movement near the edges of platforms.

Then I did the playtest on a phone with two people who had never seen my game. Zamira beat the level in 2 tries. Nurakhmet needed 4 or 5 tries. That was a bit too hard for the second player.

AI helped me understand the assignment and fix some errors, but I wrote and tested the enemy logic myself.
