# Room Visibility Management System (Unity)

---

## Overview
This project implements a **room-based visibility management system** designed for a horror adventure game targeting **low-end PCs**, including systems with integrated GPUs.  

The goal is to maintain strong atmosphere and lighting while keeping performance costs minimal.

---

## Problem Statement
The game takes place at night in an enclosed environment with:  

- Interconnected rooms  
- Doors that remain open  
- Dynamic lighting contributing to atmosphere  

Because doors stay open, rooms may remain visible — or partially visible — from adjacent spaces.  

Traditional real-time visibility checks were avoided due to:  

- Performance concerns  
- Potential visual pop-in  
- Delayed room activation breaking immersion

---

## Design Goals
- Support open-door visibility  
- Preserve lighting continuity between rooms  
- Avoid real-time culling calculations  
- Prevent visible pop-in  
- Minimize CPU & GPU overhead

---

## Core Concept
Each room declares **which other rooms can be visible from it**, and at what level of visibility.  

When the player enters a room, the `Room_Manager`:

- Evaluates visibility states  
- Enables or disables room content accordingly  
- Performs all changes instantly (no delays)

This approach trades **runtime calculations for predefined relationships**, improving **stability and performance**.

---

## Room Visibility States
Each room can be in **one of three states**:

- **FULL_VISIBLE**: Room is fully visible and fully enabled  
- **PARTIAL_VISIBLE**: Room geometry is not visible, but its lights can affect visible rooms through open doors  
- **INVISIBLE**: Room has no visual or lighting impact and is fully disabled

---

## Room Hierarchy Structure
Each Room GameObject contains **two child objects**:

    Room
    ├── Lights&Walls
    │   ├── Walls / Floor / Ceiling
    │   └── Lights that can affect other rooms
    └── Furnitures
        └── Props and decorative objects

This separation allows **fine-grained control based on visibility state**:  

- `FULL_VISIBLE` → both children enabled  
- `PARTIAL_VISIBLE` → only `Lights&Walls` enabled  
- `INVISIBLE` → both children disabled  

---

## Runtime Flow
- Each room contains a **trigger volume**  
- When the player enters a room:  
  1. The `Room_Manager` is notified  
  2. Visibility states are updated based on **predefined room relationships**  
  3. Rooms are enabled or disabled instantly  

**No continuous visibility checks are performed.**
