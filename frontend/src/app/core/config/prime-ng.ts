import { Provider, APP_INITIALIZER } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';

function initializePrimeNgFactory(primeNgConfigService: PrimeNGConfig) {
  return () => {
    primeNgConfigService.ripple = true;
  };
}

export const PRIMENG_CONFIG_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initializePrimeNgFactory,
  deps: [PrimeNGConfig],
  multi: true,
};
