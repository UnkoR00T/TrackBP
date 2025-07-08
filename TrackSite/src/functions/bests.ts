import type { PlayerData } from '@/types/playerType.ts'
type bestsType = {
  bestSectorOne: number,
    bestSectorTwo: number,
    bestSectorThree: number,
    bestLap: number,
    fastest: number,
}
function bestsFn(players: PlayerData[]): bestsType {
  const sectorOne = players.sort(x=>x.PersonalBests.Sectors.One)[0].PlayerId;
  const sectorTwo = players.sort(x=>x.PersonalBests.Sectors.Two)[0].PlayerId;
  const sectorThree = players.sort(x=>x.PersonalBests.Sectors.Three)[0].PlayerId;
  const bestLap = players.sort(x=>x.PersonalBests.LapTime)[0].PlayerId;
  const fastest = players.sort(x=>x.PersonalBests.TimeTrap)[0].PlayerId;
  return {
    bestSectorOne: sectorOne,
    bestSectorTwo: sectorTwo,
    bestSectorThree: sectorThree,
    bestLap,
    fastest,
  }
}

export {bestsFn, type bestsType}
