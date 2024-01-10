import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  readonly #isSideMenuOpen = signal(false);
  get isSideMenuOpen() {
    return this.#isSideMenuOpen.asReadonly();
  }

  readonly #isPagesMenuOpen = signal(false);

  get isPagesMenuOpen() {
    return this.#isPagesMenuOpen.asReadonly();
  }

  readonly #isThemeDark = signal(true);

  get isThemeDark() {
    return this.#isThemeDark.asReadonly();
  }

  #isProfileMenuOpen = signal(false);

  get isProfileMenuOpen() {
    return this.#isProfileMenuOpen.asReadonly();
  }

  togglePagesMenu() {
    const value = this.#isPagesMenuOpen();
    this.#isPagesMenuOpen.set(!value);
  }

  toggleSideMenu() {
    const value = this.isSideMenuOpen();
    this.#isSideMenuOpen.set(!value);
  }

  toggleTheme() {
    const value = !this.#isThemeDark();
    this.#isThemeDark.set(value);

    localStorage.setItem('theme', value ? 'dark' : 'light');
  }

  toggleProfileMenu() {
    const value = this.#isProfileMenuOpen();
    this.#isProfileMenuOpen.set(!value);
  }

  closeSideMenu() {
    this.#isSideMenuOpen.set(false);
  }

  closeProfileMenu() {
    this.#isProfileMenuOpen.set(false);
  }

  closeOpenMenus() {
    this.#isSideMenuOpen.set(false);
    this.#isProfileMenuOpen.set(false);
  }

  isMobile() {
    return window.innerWidth <= 720;
  }

  private getInitialTheme() {
    const theme = localStorage.getItem('theme');
    return theme ? theme === 'dark' : this.getUserSystemThemePreference();
  }

  private getUserSystemThemePreference() {
    return window.matchMedia('(prefers-color-scheme: dark)').matches;
  }
}
