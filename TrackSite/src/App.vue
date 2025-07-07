<script setup lang="ts">
import { usePlayersStore } from '@/stores/playersStore.ts'

const players = usePlayersStore();
players.connectToWS();
</script>

<template>
  <header>
    <nav>
      <RouterLink to="/">Live Map</RouterLink>
      <RouterLink to="/leaderboard">Leaderboard</RouterLink>
    </nav>
  </header>
  <RouterView />
</template>

<style scoped>
nav{
  margin-top: 5px;
  display: flex;
  justify-content: center;
  gap: 25px;
}
nav>a{
  z-index: 5;
  transition-duration: .2s;
  color: white;
  text-decoration: none;
  font-weight: bold;
  font-size: 1.2rem;
}
nav>a:hover{
  color: #00a19c;
}
</style>
