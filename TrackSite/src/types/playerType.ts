type SectorTimes = {
  One: number;
  Two: number;
  Three: number;
};

type PersonalBest = {
  Id: number;
  LapTime: number;
  PlayerName: string;
  Sectors: SectorTimes;
  TimeTrap: number;
};

type CustomData = {
  Data: {
    Jailtime: number;
    Offenses: string; // Ugh, string zamiast array – ktoś tu chyba jebnął serializację
    enteredSector1?: number;
    enteredSector2?: number;
    enteredSector3?: number;
    driverNumber?: number;
    driverColor?: string;
  };
};

type Position = {
  x: number;
  y: number;
  z: number;
};

type PlayerData = {
  CustomData: CustomData;
  PersonalBests: PersonalBest;
  PlayerId: number;
  PlayerName: string;
  Pos: Position;
};
type PosUpdateBody = {
  [playerName: string]: Position
}
type WSMessage = {
  title: string
  body: never
};
export type { SectorTimes, PlayerData, CustomData, Position, PersonalBest, PosUpdateBody, WSMessage };
