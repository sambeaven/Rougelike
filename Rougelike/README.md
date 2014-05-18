# Rougelike

This is my first attempt at writing a simple Roguelike. It's all very rough, and will probably remain that way!

(Yes, the typo is deliberate)

## Current Todos:

- Doors
- Unit test combat system
	- Maybe check for balance while we're at it?
- Fog of war
- Items and Equipment
- Random dungeon generation
- Random monster generation
- Monster speed (slower monsters don't move every turn, faster monsters may move twice)
- Time (certain actions take more time than others)
- Give monsters a sense of object permanence (as in, able to follow the player when they can't see them)
- Patrolling monsters
- Crafting

## Known Bugs:

- Monsters can get stuck next to each other
    - We need them to recognize when they can see the player but the destination cell is impassable, and then find a way around it.
