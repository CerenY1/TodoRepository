import { ApplicationConfig } from '@angular/core';
import { provideHttpClient,withFetch } from '@angular/common/http';
import { provideClientHydration } from '@angular/platform-browser';
import { provideZonelessChangeDetection } from '@angular/core'; 

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(), 
    provideHttpClient(withFetch()),
    provideHttpClient(),
    provideClientHydration()
  ]
};