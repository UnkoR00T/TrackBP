import type { PlayerData } from '@/types/playerType.ts'

type bestsType = {
  bestSectorOne: number,
  bestSectorTwo: number,
  bestSectorThree: number,
  bestLap: number,
  fastest: number,
  laps: number,
}

function bestsFn(players: PlayerData[]): bestsType {
  const sectorOne = [...players].sort((a, b) => a.PersonalBests.Sectors.One - b.PersonalBests.Sectors.One)[0].PlayerId;
  const sectorTwo = [...players].sort((a, b) => a.PersonalBests.Sectors.Two - b.PersonalBests.Sectors.Two)[0].PlayerId;
  const sectorThree = [...players].sort((a, b) => a.PersonalBests.Sectors.Three - b.PersonalBests.Sectors.Three)[0].PlayerId;
  const bestLap = [...players].sort((a, b) => a.PersonalBests.LapTime - b.PersonalBests.LapTime)[0].PlayerId;
  const fastest = [...players].sort((a, b) => b.PersonalBests.TimeTrap - a.PersonalBests.TimeTrap)[0].PlayerId;
  const laps = [...players].sort((a, b) => b.PersonalBests.Laps - a.PersonalBests.Laps)[0].PlayerId;

  return {
    bestSectorOne: sectorOne,
    bestSectorTwo: sectorTwo,
    bestSectorThree: sectorThree,
    bestLap,
    fastest,
    laps
  }
}

export { bestsFn, type bestsType }
