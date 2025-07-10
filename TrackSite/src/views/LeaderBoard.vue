<script setup lang="ts">
import { usePlayersStore } from '@/stores/playersStore.ts'
import { computed, onMounted, onUnmounted, ref } from 'vue'
import { doubleToDateTime, formatTime } from '@/functions/time.ts'
import {  bestsFn, type bestsType } from '@/functions/bests.ts'
import type { PlayerData } from '@/types/playerType.ts'

const playersStore = usePlayersStore();
const sessionCountdown = ref('');

function updateCountdown() {
  const now = new Date().getTime();
  const sessionEnd = doubleToDateTime(playersStore.raceInfo.SessionEndsAt).getTime();

  const diff = sessionEnd - now;

  if (diff <= 0) {
    sessionCountdown.value = "Session ended";
    return;
  }

  const hours = Math.floor(diff / (1000 * 60 * 60));
  const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((diff % (1000 * 60)) / 1000);
  let toDisplay = ``;
  if (hours > 0) toDisplay += `${hours}h`;
  if (minutes > 0) toDisplay += ` ${minutes}m`;
  if (seconds > 0) toDisplay += ` ${seconds}s`;
  sessionCountdown.value = toDisplay;
}
let intervalId: number;
onMounted(() => {
  updateCountdown();
  intervalId = setInterval(updateCountdown, 1000);
});
onUnmounted(() => {
  clearInterval(intervalId);
});
const showDrivers = computed((): {filterd: PlayerData[], bests: bestsType} => {
  const filterd = playersStore.players.filter(x => x.CustomData.Data.driverNumber && x.CustomData.Data.driverColor);
  const bests = bestsFn(filterd);
  return {filterd, bests};
})
</script>

<template>
  <main>
    <div class="playersLeaderboard">
      <header>
        <h1>Leaderboard</h1>
        <p style="color: gray;">(Online drivers)</p>
      </header>
      <div class="playersAndTitle">
        <div class="player title" style="color: gray; font-size: 10px; text-align: center; flex-shrink: 0;">
          <div class="position" style="font-size: 10px;">
          </div>
          <div class="username" style="font-size: 10px;">
            <span>
              Name
            </span>
            <span style="font-size: 10px; color: gray;">
            (no)
          </span>
          </div>
          <div class="team">
            Team
          </div>
          <div class="personalBests">
            <div class="speedtrap">
              SpeedTrap
            </div>
            <div class="sectors">
              <div class="sector" id="sectorOne">
                Sector 1
              </div>
              <div class="sector" id="sectorTwo">
                Sector 2
              </div>
              <div class="sector" id="sectorThree">
                Sector 3
              </div>
            </div>
            <div class="laptime">
              Laptime
            </div>
          </div>
        </div>
        <div class="player" v-for="(player, i) in showDrivers.filterd.sort(x=> x.PersonalBests.LapTime)" :key="player.PlayerId">
          <div class="position">
            {{i + 1}}
          </div>
          <div class="username">
          <span>
            {{player.PlayerName}}
          </span>
          <span style="font-size: 12px; color: gray;">
            ({{player.CustomData.Data.driverNumber}})
          </span>
          </div>
          <div class="team">
            <div class="teamColor" style="height: 30px; width: 50px;" :style="{backgroundColor: '#' + player.CustomData.Data.driverColor}"></div>
          </div>
          <div class="personalBests">
            <div class="speedtrap" :style="{color: showDrivers.bests.fastest == player.PlayerId ? '#83018c' : '#fafafa'}">
              <span v-if="player.PersonalBests.TimeTrap > 5">{{player.PersonalBests.TimeTrap.toFixed(2) + "km/h"}}</span>
              <span v-else>---</span>
            </div>
            <div class="sectors">
              <div class="sector" id="sectorOne" :style="{color: showDrivers.bests.bestSectorOne == player.PlayerId ? '#83018c' : '#fafafa'}">
                <span v-if="player.PersonalBests.Sectors.One < 5000">{{formatTime(player.PersonalBests.Sectors.One)}}</span>
                <span v-else>---</span>
              </div>
              <div class="sector" id="sectorTwo" :style="{color: showDrivers.bests.bestSectorTwo == player.PlayerId ? '#83018c' : '#fafafa'}">
                <span v-if="player.PersonalBests.Sectors.Two < 5000">{{formatTime(player.PersonalBests.Sectors.Two)}}</span>
                <span v-else>---</span>
              </div>
              <div class="sector" id="sectorThree" :style="{color: showDrivers.bests.bestSectorThree == player.PlayerId ? '#83018c' : '#fafafa'}">
                <span v-if="player.PersonalBests.Sectors.Three < 5000">{{formatTime(player.PersonalBests.Sectors.Three)}}</span>
                <span v-else>---</span>
              </div>
            </div>
            <div class="laptime" :style="{color: showDrivers.bests.bestLap == player.PlayerId ? '#83018c' : '#fafafa'}">
              <span v-if="player.PersonalBests.LapTime < 5000">{{formatTime(player.PersonalBests.LapTime)}}</span>
              <span v-else>---</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="raceInfo">
      <header>
        <h1>Race Info</h1>
        <p style="color: gray;">(Current session)</p>
      </header>
      <div class="content">
        <div class="raceSession">
          <span>Session: </span>
          {{playersStore.raceInfo.Session}}
        </div>
        <div class="raceSessionEndsAt">
          <span>Session ends in: </span>
          {{sessionCountdown}}
        </div>
        <div class="drs">
          <span>DRS Status:</span>
          {{playersStore.raceInfo.DRS ? "Enabled" : "Disabled"}}
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
*{
  flex-shrink: 0;
}
.personalBests {
  margin-left: 30px;
  display: flex;
  gap: 25px;
}
.personalBests>*>*{
  width: 100px;
}
.position{
  margin: 0px 25px;
  font-size: 24px;
  width: 25px;
}
.player{
  display: flex;
  height: 30px;
  line-height: 30px;
}
.playersAndTitle{
  overflow-x: auto;
}
.username{
  font-size: 18px;
  display: flex;
  gap: 5px;
  width: 175px;
}
.sectors{
  display: flex;
  gap: 25px;
}
.teamColor{
  margin: 0px 25px;
  transform:matrix(1.00,0.00,-1.00,1.00,0,0);
  -ms-transform:matrix(1.00,0.00,-1.00,1.00,0,0);
  -webkit-transform:matrix(1.00,0.00,-1.00,1.00,0,0);
  flex-shrink: 0;
}
.team{
  width: 100px;
}
*{
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
}
.speedtrap{
  width: 90px;
}
.laptime{
  width: 90px;
}
main>div>header>*{
  margin: 0;
}
main>div>header{
  margin-bottom: 25px;
}
main>div{
  padding: 25px;
  background-color: rgba(0, 0, 0, 0.2);
  border-radius: 15px;
}
.raceInfo{
  padding: 25px;
}
header{
  text-align: center;
}
main{
  display: flex;
  gap: 25px;
  justify-content: center;
  margin-top: 25px;
}
@media screen and (max-width: 1300px) {
  main{
    flex-direction: column;
    align-items: center;
    justify-content: start;
  }
  main>div{
    width: 90dvw;
  }
}
</style>
