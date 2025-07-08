import { defineStore } from 'pinia'
import type { PlayerData, PosUpdateBody, WSMessage } from '@/types/playerType.ts'
import { ref, type Ref } from 'vue'
import axios from 'axios'
import type { RaceInfo } from '@/types/raceType.ts'

export const usePlayersStore = defineStore('players', () => {
  const players: Ref<PlayerData[]> = ref([])
  const raceInfo: Ref<RaceInfo> = ref({
    DRS: false,
    Session: "P 0",
    SessionEndsAt: 0
  });
  const connectToWS = async () => {
    try {
      let response = await axios.get('http://135.181.216.103:14430/get_players')
      players.value = response.data
      response = await axios.get('http://135.181.216.103:14430/get_raceinfo')
      raceInfo.value = response.data
    } catch (err) {
      console.error('Chujowa odpowiedź z serwera:', err)
    }
    const ws = new WebSocket(`ws://135.181.216.103:14430/ws`)
    ws.onmessage = (e: MessageEvent) => {
      let msg: WSMessage
      try {
        msg = JSON.parse(e.data)
      } catch {
        return
      }
      if (msg.title === 'posUpdate') {
        const data = msg.body as PosUpdateBody
        for (const playerName in data) {
          const newPos = data[playerName]
          const player = players.value.find((p) => p.PlayerName === playerName)
          if (player) {
            player.Pos = newPos
          }
        }
      }
      if (msg.title === 'playerJoin') {
        players.value.push(msg.body as PlayerData)
        console.log('Player added:', msg.body)
      }
      if (msg.title === 'playerLeave') {
        players.value = players.value.filter((player) => player.PlayerName !== msg.body)
      }
      if (msg.title === 'playerUpdate') {
        const data = msg.body as PlayerData
        const index = players.value.findIndex((p) => p.PlayerName === data.PlayerName)
        if (index !== -1) {
          players.value[index] = data
        }
      }
      if (msg.title === 'raceInfoUpdate') {
        raceInfo.value = msg.body as RaceInfo
      }
    }
  }
  return { connectToWS, players, raceInfo }
})
