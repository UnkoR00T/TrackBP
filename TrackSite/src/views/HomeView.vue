<script setup lang="ts">
import type { PlayerData } from '@/types/playerType.ts'
import { computed, type CSSProperties, onBeforeUnmount, onMounted, type Ref, ref } from 'vue'
import { usePlayersStore } from '@/stores/playersStore.ts'

const playersStore = usePlayersStore();
const mapSvg = ref<SVGElement | null>(null);
const windowSizeTrigger: Ref<number> = ref(0)


const showDrivers = computed(() => {
  return playersStore.players.filter(x => x.CustomData.Data.driverNumber && x.CustomData.Data.driverColor)
})

function triggerResizeUpdate() {
  windowSizeTrigger.value = Date.now()
}

onMounted(() => {
  window.addEventListener('resize', triggerResizeUpdate)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', triggerResizeUpdate)
})

function unityToMapCoords(unityX: number, unityZ: number, svgRect: DOMRect) {

  const minX = -1226;
  const maxX = 1236;
  const minZ = -671;
  const maxZ = 673;
  const paddingX = 0.04;
  let normalizedX = 1- (unityX - minX) / (maxX - minX);
  normalizedX = paddingX + normalizedX * (1 - 2 * paddingX);
  const normalizedY = 1- ((unityZ - minZ) / (maxZ - minZ));

  const pixelX = normalizedX * svgRect.width + svgRect.left;
  const pixelY = normalizedY * svgRect.height + svgRect.top - 30;

  return {
    x: Math.round(pixelX),
    y: Math.round(pixelY)
  };
}


function getPlayerStyle(player: PlayerData): CSSProperties {
  // eslint-disable-next-line @typescript-eslint/no-unused-expressions
  windowSizeTrigger.value;
  if (!mapSvg.value) return {};

  const rect = mapSvg.value.getBoundingClientRect();
  const coords = unityToMapCoords(player.Pos.z, player.Pos.x, rect);

  return {
    position: 'absolute',
    left: `${coords.x}px`,
    top: `${coords.y}px`,
    width: '30px',
    height: '30px',
    borderRadius: '50%',
    background: `#${player.CustomData.Data.driverColor}`,
    zIndex: 2,
    transform: 'translate(0, -15px)'
  };
}
</script>

<template>

  <div ref="mapContainer" class="map-container" id='liveMap'>
    <svg
      ref="mapSvg"
      viewBox="0 0 553 274"
      xmlns="http://www.w3.org/2000/svg"
      preserveAspectRatio="xMidYMid meet"
      class="map-svg"
    >
      <image href="/track.svg" width="100%" height="100%" />
    </svg>

    <div
      v-for="player in showDrivers"
      :key="player.PlayerId"
      class="player"
      :style="getPlayerStyle(player)">
      <span class="driver">{{ player.CustomData.Data.driverNumber }}</span>
    </div>
  </div>
</template>

<style scoped>
.map-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
}

.map-svg {
  width: 75vw;
  height: auto;
  display: block;
  position: relative;
}

.player {
  pointer-events: none;
  text-align: center;
  color: white;
  font-weight: bold;
  line-height: 30px;
}
span.driver {
  position: relative;
  width: 30px;
  height: 30px;
  line-height: 30px;
  text-align: center;
  display: block;
  font-weight: bold;
  color: black;
  user-select: none;
  pointer-events: none;

}
</style>
