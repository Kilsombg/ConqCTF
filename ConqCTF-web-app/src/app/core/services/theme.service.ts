import { Injectable } from '@angular/core';

type ThemeMode = 'light' | 'dark';

@Injectable({
    providedIn: 'root'
})
export class ThemeService {
    private readonly storageKey = 'theme';
    private readonly darkClass = 'dark-theme';
    private readonly lightClass = 'light-theme';

    constructor() {
        this.loadTheme();
    }

    setDark(): void {
        this.applyTheme('dark');
    }

    setLight(): void {
        this.applyTheme('light');
    }

    toggle(): void {
        this.applyTheme(this.isDark() ? 'light' : 'dark');
    }

    private applyTheme(theme: ThemeMode): void {
        document.body.classList.remove(this.darkClass, this.lightClass);
        document.body.classList.add(
            theme === 'dark' ? this.darkClass : this.lightClass
        );

        localStorage.setItem(this.storageKey, theme);
    }

    private loadTheme(): void {
        const savedTheme = localStorage.getItem(this.storageKey) as ThemeMode | null;

        if (savedTheme === 'dark' || savedTheme === 'light') {
            this.applyTheme(savedTheme);
        } else {
            this.applyTheme('light');
        }
    }

    isDark(): boolean {
        return document.body.classList.contains(this.darkClass);
    }
}
