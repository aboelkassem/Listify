import '@types/youtube';
// import {} from '@types/youtube';

export interface PlayerConfig {
  elementId: string;
  width: number;
  height: number;
  videoId?: string;
  outputs: {
    ready: any,
    change: any
  };
}
