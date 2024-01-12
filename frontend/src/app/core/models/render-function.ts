import { Entity } from './entity';
export type RenderFunction<T extends Entity> = (
  entity: T,
  key: keyof T
) => boolean;
