# 🎨 UX/UI Detaljni Report - Web Aplikacija za Rezervacije Prostora za Probe

**Datum:** 2026-05-05 00:53:03  
**Agent:** UX/UI Sub-agent za ASP.NET Core MVC  
**Projekt:** Lab2.RezervacijeProstora  
**Verzija:** 1.0

---

## 📋 SADRŽAJ IZVJEŠTAJA

1. [Analiza Trenutnog Stanja](#analiza-trenutnog-stanja)
2. [Home Page Preporuke](#home-page-preporuke)
3. [Navigacija Preporuke](#navigacija-preporuke)
4. [Card Layout Preporuke](#card-layout-preporuke)
5. [Details Stranice Preporuke](#details-stranice-preporuke)
6. [Breadcrumbs Preporuke](#breadcrumbs-preporuke)
7. [Boje i Kontrast Preporuke](#boje-i-kontrast-preporuke)
8. [Akcijski Plan](#akcijski-plan)

---

## ✅ ANALIZA TRENUTNOG STANJA

### Što je već dobro:

#### 1. Vizualni Identitet
- ✅ Tamna pozadina je konzistentna (#0f1117, #171a23)
- ✅ Ljubičasti akcenti (#7c3aed) daju prepoznatljiv studio/glazbeni stil
- ✅ Neon naglasci (#a78bfa) na critical elementima
- ✅ Odličan kontrast između pozadine i teksta za većinu elemenata

#### 2. Layout Struktura
- ✅ CSS Grid layout je responsive i fleksibilan
- ✅ Kartice koriste `auto-fit` s minmax za adaptivnost
- ✅ Hero sekcija ima dobru vizualnu težinu
- ✅ Sekcije su jasno organizirane s max-width containerom

#### 3. Tipografija
- ✅ Tekst je čitljiv na tamnoj pozadini
- ✅ `eyebrow` elementi jasno označavaju sekcije
- ✅ Hijerarhija h1 > h2 > h3 je logična
- ✅ Font-weight razlike se koriste za naglašavanje

#### 4. Komponente
- ✅ Stat cards za Key Performance Indicators
- ✅ Custom cards za objekte (prostori, recenzije)
- ✅ Details paneli za organiziranje informacija
- ✅ Breadcrumbs navigacija na Details stranicama
- ✅ Timeline list za aktivnosti
- ✅ Clean list za formatiranje podataka

### Što nedostaje ili može biti bolje:

#### 1. Interaktivnost
- ❌ Kartice imaju minimal hover efekt
- ❌ Gumbi bez focus-visible state-ova
- ❌ Nema transition animacija na većini elemenata
- ❌ Nema loading/disabled state-ova

#### 2. Vizualna Hijerarhija
- ❌ Details paneli izgledaju svi isto
- ❌ Nema visual cue-ova za "currently active" stranicu
- ❌ Status informacije nisu color-coded
- ❌ Prioriteta između elemenata nije jasna

#### 3. Accessibility
- ❌ Nedostaju focus outline-ovi
- ⚠️ Kontrast je dobar, ali može biti poboljšan na nekim mjestima
- ❌ Nema `aria-label` ili `role` atributa u prikazanom kodu

#### 4. Responsive Design
- ⚠️ Hero sekcija padding može biti smanjen na mobilnim
- ⚠️ Navigacija je previše skučena na mobilnim resolutions
- ⚠️ Card grid mogu biti jednostavnije na mobilnom

---

## 🏠 HOME PAGE PREPORUKE

### Trenutno Stanje:
```
✅ Hero sekcija s gradientom
✅ Stat cards (4 stupca)
✅ Featured prostori sekcija
✅ Zadnje rezervacije timeline
```

### PREPORUKA 1.1: Hero Sekcija - Dodati Ambijentalne Animacije

**Gdje:** Home/Index.cshtml > hero-section  
**Što:** Dodati animirane background elemente za dinamičniji izgled

**CSS:**
```css
.hero-content {
    position: relative;
    overflow: hidden;
}

/* Animated floating element */
.hero-content::before {
    content: '';
    position: absolute;
    top: -50%;
    right: -10%;
    width: 600px;
    height: 600px;
    background: radial-gradient(circle at center, rgba(124, 58, 237, 0.15) 0%, rgba(124, 58, 237, 0.05) 50%, transparent 70%);
    border-radius: 50%;
    animation: float 8s ease-in-out infinite;
    pointer-events: none;
    z-index: 0;
}

.hero-content::after {
    content: '';
    position: absolute;
    bottom: -30%;
    left: -5%;
    width: 500px;
    height: 500px;
    background: radial-gradient(circle at center, rgba(167, 139, 250, 0.1) 0%, transparent 70%);
    border-radius: 50%;
    animation: float-reverse 10s ease-in-out infinite;
    pointer-events: none;
    z-index: 0;
}

@keyframes float {
    0%, 100% { 
        transform: translateY(0px) translateX(0px);
        opacity: 0.8;
    }
    25% { 
        transform: translateY(-30px) translateX(20px);
        opacity: 0.9;
    }
    50% { 
        transform: translateY(-60px) translateX(-10px);
        opacity: 0.8;
    }
    75% { 
        transform: translateY(-30px) translateX(10px);
        opacity: 0.7;
    }
}

@keyframes float-reverse {
    0%, 100% { 
        transform: translateY(0px) translateX(0px);
        opacity: 0.6;
    }
    50% { 
        transform: translateY(40px) translateX(-20px);
        opacity: 0.8;
    }
}

/* Osiguranje da se sadržaj prikazuje ispred animacija */
.hero-content > * {
    position: relative;
    z-index: 1;
}
```

**Rezultat:** Dinamičan, energičan izgled bez da se mijenja funkcionalnost.

---

### PREPORUKA 1.2: Stat Cards - Vizualni Identitet

**Gdje:** Home/Index.cshtml > stats-grid  
**Što:** Individualizirati stat cards s bojama i ikonama

**HTML (primjer za Index.cshtml):**
```html
<section class="stats-grid">
    <div class="stat-card stat-card-spaces">
        <div class="stat-icon">🏢</div>
        <span>@Model.BrojProstora</span>
        <p>prostora za probu</p>
    </div>

    <div class="stat-card stat-card-users">
        <div class="stat-icon">👥</div>
        <span>@Model.BrojKorisnika</span>
        <p>aktivnih korisnika</p>
    </div>

    <div class="stat-card stat-card-bookings">
        <div class="stat-icon">📅</div>
        <span>@Model.BrojRezervacija</span>
        <p>rezervacija</p>
    </div>

    <div class="stat-card stat-card-equipment">
        <div class="stat-icon">🎸</div>
        <span>@Model.BrojOpreme</span>
        <p>komada opreme</p>
    </div>
</section>
```

**CSS:**
```css
.stat-card {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
    background: linear-gradient(135deg, #171a23 0%, #1a1e2e 100%);
    border: 1px solid #2b3040;
    border-radius: 16px;
    padding: 24px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    position: relative;
    overflow: hidden;
}

.stat-card::before {
    content: '';
    position: absolute;
    top: -50%;
    right: -50%;
    width: 200%;
    height: 200%;
    background: linear-gradient(45deg, transparent 30%, rgba(124, 58, 237, 0.1) 50%, transparent 70%);
    transform: rotate(45deg);
    animation: shine 3s infinite;
    pointer-events: none;
}

@keyframes shine {
    0% { transform: translateX(-100%) translateY(-100%) rotate(45deg); }
    100% { transform: translateX(100%) translateY(100%) rotate(45deg); }
}

.stat-icon {
    font-size: 48px;
    margin-bottom: 12px;
    display: block;
    animation: bounce 2s ease-in-out infinite;
}

@keyframes bounce {
    0%, 100% { transform: translateY(0); }
    50% { transform: translateY(-10px); }
}

.stat-card span {
    font-size: 36px;
    font-weight: bold;
    color: #a78bfa;
}

.stat-card p {
    color: #c4c4cc;
    font-size: 14px;
    margin: 8px 0 0 0;
}

.stat-card:hover {
    border-color: #7c3aed;
    transform: translateY(-8px);
    box-shadow: 0 12px 24px rgba(124, 58, 237, 0.2);
}

/* Type-specific colors */
.stat-card-spaces:hover {
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(124, 58, 237, 0.1) 100%);
}

.stat-card-users:hover {
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(16, 185, 129, 0.1) 100%);
}

.stat-card-bookings:hover {
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(245, 158, 11, 0.1) 100%);
}

.stat-card-equipment:hover {
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(236, 72, 153, 0.1) 100%);
}
```

**Rezultat:** Svaki stat card ima unikatan identitet, ali ostaje dio design sistema.

---

### PREPORUKA 1.3: CTA Gumbi - Poboljšana Interaktivnost

**CSS:**
```css
.hero-actions {
    display: flex;
    gap: 16px;
    flex-wrap: wrap;
    margin-top: 24px;
}

.primary-button,
.secondary-button {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    padding: 14px 28px;
    border-radius: 12px;
    text-decoration: none;
    font-weight: 600;
    font-size: 15px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    border: 2px solid transparent;
    cursor: pointer;
    position: relative;
    overflow: hidden;
}

.primary-button {
    background: #7c3aed;
    color: white;
}

.primary-button::before {
    content: '';
    position: absolute;
    top: 50%;
    left: 50%;
    width: 0;
    height: 0;
    background: rgba(255, 255, 255, 0.2);
    border-radius: 50%;
    transform: translate(-50%, -50%);
    transition: width 0.6s, height 0.6s;
}

.primary-button:hover::before {
    width: 300px;
    height: 300px;
}

.primary-button:hover {
    background: #8b5cf6;
    transform: translateY(-2px);
    box-shadow: 0 8px 16px rgba(124, 58, 237, 0.3);
}

.primary-button:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

.secondary-button {
    background: transparent;
    color: #f4f4f5;
    border: 2px solid #272b3a;
}

.secondary-button:hover {
    border-color: #7c3aed;
    background: rgba(124, 58, 237, 0.1);
    transform: translateY(-2px);
}

.secondary-button:focus-visible {
    outline: 2px solid #7c3aed;
    outline-offset: 2px;
}
```

---

## 📱 NAVIGACIJA PREPORUKE

### Trenutno Stanje:
```
- Fleksibilan layout s flex-wrap
- Svi linkovi imaju isti stil
- Nema primarne grupe ili organizacije
- Hover efekt samo promjena boje
```

### PREPORUKA 2.1: Reorganizacija Navigation u Grupe

**HTML za _Layout.cshtml:**
```html
<header>
    <nav class="main-nav">
        <div class="nav-section">
            <span class="nav-section-label">PREGLED</span>
            <a asp-controller="Home" asp-action="Index">
                <span class="nav-icon">🏠</span>
                Početna
            </a>
            <a asp-controller="ProstorZaProbu" asp-action="Index">
                <span class="nav-icon">🏢</span>
                Prostori
            </a>
            <a asp-controller="Korisnik" asp-action="Index">
                <span class="nav-icon">👥</span>
                Korisnici
            </a>
        </div>

        <div class="nav-section">
            <span class="nav-section-label">AKTIVNOSTI</span>
            <a asp-controller="Rezervacija" asp-action="Index">
                <span class="nav-icon">📅</span>
                Rezervacije
            </a>
            <a asp-controller="Placanje" asp-action="Index">
                <span class="nav-icon">💳</span>
                Plaćanja
            </a>
            <a asp-controller="Recenzija" asp-action="Index">
                <span class="nav-icon">⭐</span>
                Recenzije
            </a>
        </div>

        <div class="nav-section">
            <span class="nav-section-label">SUSTAV</span>
            <a asp-controller="Oprema" asp-action="Index">
                <span class="nav-icon">🎸</span>
                Oprema
            </a>
            <a asp-controller="Vlasnik" asp-action="Index">
                <span class="nav-icon">👤</span>
                Vlasnici
            </a>
            <a asp-controller="Lokacija" asp-action="Index">
                <span class="nav-icon">📍</span>
                Lokacije
            </a>
        </div>
    </nav>
</header>
```

**CSS za grupirane navigacije:**
```css
.main-nav {
    display: flex;
    gap: 32px;
    flex-wrap: wrap;
    padding: 16px 32px;
    background: #171a23;
    border-bottom: 2px solid #7c3aed;
    align-items: flex-start;
}

.nav-section {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.nav-section-label {
    color: #7c3aed;
    font-size: 11px;
    font-weight: 700;
    letter-spacing: 1.5px;
    text-transform: uppercase;
    padding: 0 14px;
    margin-bottom: 4px;
}

.nav-section a {
    display: flex;
    align-items: center;
    gap: 10px;
    color: #c4c4cc;
    text-decoration: none;
    padding: 10px 14px;
    border-radius: 8px;
    border-left: 3px solid transparent;
    transition: all 0.2s ease;
    font-size: 14px;
}

.nav-section a .nav-icon {
    display: inline-block;
    width: 20px;
    text-align: center;
}

.nav-section a:hover {
    background: rgba(124, 58, 237, 0.2);
    color: #f4f4f5;
    border-left-color: #7c3aed;
    transform: translateX(4px);
}

.nav-section a:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

/* Responsivno na mobilnom */
@media (max-width: 768px) {
    .main-nav {
        gap: 16px;
        flex-direction: column;
    }

    .nav-section {
        flex-direction: row;
        flex-wrap: wrap;
        gap: 4px;
    }

    .nav-section-label {
        display: none;
    }
}
```

---

### PREPORUKA 2.2: Active Link State

**HTML (potreban helper ili C# logic):**
```html
<a asp-controller="ProstorZaProbu" asp-action="Index" 
   class="nav-section a @(ViewContext.RouteData.Values["controller"]?.ToString() == "ProstorZaProbu" ? "active" : "")">
    <span class="nav-icon">🏢</span>
    Prostori
</a>
```

**CSS:**
```css
.nav-section a.active {
    background: linear-gradient(90deg, rgba(124, 58, 237, 0.3), transparent);
    color: #a78bfa;
    border-left-color: #a78bfa;
    font-weight: 600;
}

.nav-section a.active::before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    bottom: 0;
    width: 3px;
    background: linear-gradient(180deg, #7c3aed, #a78bfa);
    border-radius: 3px;
}
```

---

## 💳 CARD LAYOUT PREPORUKE

### Trenutno Stanje:
```
✅ CSS Grid s auto-fit
✅ Osnovni hover efekt
⚠️ Nema dinamičnog sadržaja
❌ Nema statusnih badgeova na kartici
❌ Nema clear CTA-a
```

### PREPORUKA 3.1: Enhanced Card Styling

**CSS:**
```css
.custom-card {
    background: linear-gradient(135deg, #171a23 0%, #1a1e2e 100%);
    border: 1px solid #2b3040;
    border-radius: 16px;
    padding: 20px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    position: relative;
    overflow: hidden;
    display: flex;
    flex-direction: column;
    min-height: 320px;
}

/* Shimmer effect na hover */
.custom-card::before {
    content: '';
    position: absolute;
    top: -50%;
    left: -50%;
    width: 200%;
    height: 200%;
    background: linear-gradient(
        45deg,
        transparent 30%,
        rgba(124, 58, 237, 0.1) 50%,
        transparent 70%
    );
    transform: rotate(45deg);
    animation: shimmer 3s infinite;
    opacity: 0;
    transition: opacity 0.3s ease;
}

@keyframes shimmer {
    0% {
        transform: translateX(-100%) translateY(-100%) rotate(45deg);
    }
    100% {
        transform: translateX(100%) translateY(100%) rotate(45deg);
    }
}

.custom-card:hover::before {
    opacity: 1;
}

.custom-card > * {
    position: relative;
    z-index: 2;
}

.custom-card h3 {
    color: #f4f4f5;
    font-size: 18px;
    margin: 0 0 8px 0;
    transition: color 0.3s ease;
    line-height: 1.4;
}

.custom-card:hover h3 {
    color: #a78bfa;
}

.custom-card p {
    color: #c4c4cc;
    font-size: 13px;
    margin: 0 0 12px 0;
}

.custom-card:hover {
    border-color: #7c3aed;
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(124, 58, 237, 0.15) 100%);
    transform: translateY(-6px);
    box-shadow: 0 16px 32px rgba(124, 58, 237, 0.15);
}

.card-meta {
    display: flex;
    gap: 8px;
    flex-wrap: wrap;
    margin: 12px 0 16px 0;
}

.card-meta span,
.tag {
    background: #272b3a;
    padding: 6px 10px;
    border-radius: 999px;
    font-size: 12px;
    color: #a1a1aa;
    border: 1px solid #2b3040;
    transition: all 0.2s ease;
}

.card-meta span:hover,
.tag:hover {
    background: rgba(124, 58, 237, 0.2);
    border-color: #7c3aed;
    color: #a78bfa;
}

.tag-row {
    display: flex;
    gap: 8px;
    flex-wrap: wrap;
    margin: 12px 0;
}

.tag {
    color: #c4b5fd;
}

.tag.active {
    background: rgba(16, 185, 129, 0.2);
    border-color: #10b981;
    color: #10b981;
}

.tag.inactive {
    background: rgba(239, 68, 68, 0.2);
    border-color: #ef4444;
    color: #fca5a5;
}

.details-link {
    margin-top: auto;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 6px;
    padding: 12px 16px;
    background: #7c3aed;
    color: white;
    text-decoration: none;
    border-radius: 10px;
    font-weight: 600;
    font-size: 13px;
    transition: all 0.3s ease;
    border: 2px solid #7c3aed;
}

.details-link:hover {
    background: #8b5cf6;
    transform: translateY(-2px);
    box-shadow: 0 8px 16px rgba(124, 58, 237, 0.3);
}

.details-link:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

.details-link::after {
    content: '→';
    transition: transform 0.3s ease;
}

.details-link:hover::after {
    transform: translateX(4px);
}
```

---

### PREPORUKA 3.2: Card Badge za Status

**HTML (za ProstorZaProbu/Index.cshtml):**
```html
<article class="custom-card">
    @if (!prostor.Aktivan)
    {
        <div class="card-badge inactive">Neaktivan</div>
    }

    <h3>@prostor.Naziv</h3>
    <p>@prostor.Lokacija.Grad, @prostor.Lokacija.Adresa</p>

    <!-- Ostatak sadržaja -->
</article>
```

**CSS:**
```css
.card-badge {
    position: absolute;
    top: 16px;
    right: 16px;
    padding: 6px 12px;
    border-radius: 999px;
    font-size: 11px;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    z-index: 3;
}

.card-badge.active {
    background: rgba(16, 185, 129, 0.2);
    color: #10b981;
    border: 1px solid #10b981;
}

.card-badge.inactive {
    background: rgba(239, 68, 68, 0.2);
    color: #ef4444;
    border: 1px solid #ef4444;
}
```

---

## 📄 DETAILS STRANICE PREPORUKE

### Trenutno Stanje:
```
✅ Breadcrumbs
✅ Hero section
✅ Panel grid
❌ Paneli nemaju vizualne razlike
❌ Nema focus state-ova
```

### PREPORUKA 4.1: Breadcrumbs Poboljšanja

**CSS:**
```css
.breadcrumbs {
    display: flex;
    align-items: center;
    gap: 12px;
    font-size: 13px;
    color: #a1a1aa;
    margin: 24px auto 0;
    padding: 0 20px;
    max-width: 1100px;
}

.breadcrumbs a {
    color: #a78bfa;
    text-decoration: none;
    transition: all 0.2s ease;
    padding: 4px 6px;
    border-radius: 4px;
}

.breadcrumbs a:hover {
    color: #f4f4f5;
    background: rgba(124, 58, 237, 0.2);
}

.breadcrumbs a:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

.breadcrumbs span {
    color: #2b3040;
}

.breadcrumbs span:last-child {
    color: #f4f4f5;
    font-weight: 600;
}
```

---

### PREPORUKA 4.2: Enhanced Details Hero

**CSS:**
```css
.details-hero {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    gap: 40px;
    background: linear-gradient(135deg, #1f2333 0%, #351c4d 100%);
    border: 2px solid #7c3aed;
    border-radius: 20px;
    padding: 40px;
    position: relative;
    overflow: hidden;
}

.details-hero::before {
    content: '';
    position: absolute;
    top: -50%;
    right: -20%;
    width: 400px;
    height: 400px;
    background: radial-gradient(circle, rgba(124, 58, 237, 0.1) 0%, transparent 70%);
    border-radius: 50%;
    animation: float 10s ease-in-out infinite;
    pointer-events: none;
}

.details-hero > * {
    position: relative;
    z-index: 1;
}

.details-hero h1 {
    color: #f4f4f5;
    font-size: 36px;
    margin: 12px 0 8px 0;
}

.details-hero > div:first-child {
    flex: 1;
}

.price-badge {
    background: linear-gradient(135deg, #7c3aed, #a78bfa);
    padding: 20px 28px;
    border-radius: 14px;
    font-weight: 700;
    font-size: 20px;
    color: white;
    text-align: center;
    white-space: nowrap;
    box-shadow: 0 8px 16px rgba(124, 58, 237, 0.3);
}

@media (max-width: 768px) {
    .details-hero {
        flex-direction: column;
        gap: 24px;
    }

    .price-badge {
        width: 100%;
    }
}
```

---

### PREPORUKA 4.3: Details Panels s Vizualnom Hijerarhijom

**CSS:**
```css
.details-panel {
    background: linear-gradient(135deg, #171a23 0%, #1a1e2e 100%);
    border: 1px solid #2b3040;
    border-radius: 16px;
    padding: 28px;
    position: relative;
    overflow: hidden;
    transition: all 0.3s ease;
}

/* Left accent border */
.details-panel::before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    bottom: 0;
    width: 5px;
    background: linear-gradient(180deg, #7c3aed, #a78bfa);
    border-radius: 16px 0 0 16px;
    transition: width 0.3s ease;
}

.details-panel:hover {
    border-color: #7c3aed;
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(124, 58, 237, 0.1) 100%);
    transform: translateX(4px);
    box-shadow: 0 8px 24px rgba(124, 58, 237, 0.1);
}

.details-panel:hover::before {
    width: 8px;
}

.details-panel h2 {
    color: #f4f4f5;
    font-size: 18px;
    margin: 0 0 16px 0;
    padding-left: 12px;
    border-bottom: 1px solid #2b3040;
    padding-bottom: 12px;
}

.details-panel p {
    color: #c4c4cc;
    font-size: 14px;
    margin: 12px 0;
    padding-left: 12px;
}

.details-panel strong {
    color: #f4f4f5;
}

.clean-list {
    list-style: none;
    padding: 0;
    margin: 0;
}

.clean-list li {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 12px;
    padding-left: 24px;
    border-bottom: 1px solid #2b3040;
    color: #c4c4cc;
    font-size: 14px;
}

.clean-list li:last-child {
    border-bottom: none;
}

.clean-list li:hover {
    background: rgba(124, 58, 237, 0.1);
}

.clean-list li span {
    color: #a78bfa;
    font-weight: 500;
}
```

---

### PREPORUKA 4.4: Back Link Poboljšanja

**CSS:**
```css
.back-link {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 12px 20px;
    background: #272b3a;
    color: #f4f4f5;
    text-decoration: none;
    border-radius: 10px;
    font-weight: 500;
    font-size: 14px;
    transition: all 0.3s ease;
    border: 2px solid #272b3a;
    margin-top: 40px;
    max-width: 1100px;
    margin-left: auto;
    margin-right: auto;
}

.back-link::before {
    content: '←';
    display: inline-block;
    transition: transform 0.3s ease;
}

.back-link:hover {
    background: #2b3040;
    border-color: #7c3aed;
    color: #a78bfa;
    transform: translateX(-4px);
}

.back-link:hover::before {
    transform: translateX(-4px);
}

.back-link:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}
```

---

## 🍞 BREADCRUMBS SPECIFIČNE PREPORUKE

### PREPORUKA 6.1: Strukturirani Breadcrumbs s Ikonama

**HTML (primjer):**
```html
<nav class="breadcrumbs" aria-label="Breadcrumb navigation">
    <a asp-controller="Home" asp-action="Index" title="Početna stranica">
        <span class="bc-icon">🏠</span>
        <span>Početna</span>
    </a>
    <span class="bc-separator">/</span>
    <a asp-controller="ProstorZaProbu" asp-action="Index" title="Pregled svih prostora">
        <span class="bc-icon">🏢</span>
        <span>Prostori</span>
    </a>
    <span class="bc-separator">/</span>
    <span class="bc-current" aria-current="page">
        <span class="bc-icon">📌</span>
        <span>@Model.Naziv</span>
    </span>
</nav>
```

**CSS:**
```css
.breadcrumbs {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 13px;
    color: #a1a1aa;
    margin: 24px auto 0;
    padding: 0 20px;
    max-width: 1100px;
    flex-wrap: wrap;
}

.breadcrumbs a,
.breadcrumbs span {
    display: inline-flex;
    align-items: center;
    gap: 6px;
}

.breadcrumbs a {
    color: #a78bfa;
    text-decoration: none;
    transition: all 0.2s ease;
    padding: 4px 8px;
    border-radius: 6px;
}

.breadcrumbs a:hover {
    color: #f4f4f5;
    background: rgba(124, 58, 237, 0.2);
}

.breadcrumbs a:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

.bc-icon {
    display: inline-block;
    width: 16px;
    text-align: center;
    opacity: 0.8;
}

.bc-separator {
    color: #2b3040;
    margin: 0 2px;
}

.bc-current {
    color: #f4f4f5;
    font-weight: 600;
    padding: 4px 8px;
}

/* Responsive */
@media (max-width: 640px) {
    .breadcrumbs {
        gap: 4px;
        font-size: 12px;
    }

    .bc-separator {
        margin: 0 2px;
    }
}
```

---

## 🎨 BOJE I KONTRAST PREPORUKE

### Trenutna Paleta:

```
PRIMARY BACKGROUND:    #0f1117 (Very Dark Navy)
SECONDARY BG:          #171a23 (Dark Navy)
TERTIARY BG:           #1a1e2e (Darker Navy)
ACCENT BG:             #222635 (Dark Gray)
HOVER BG:              #272b3a (Medium Gray)
BORDER:                #2b3040 (Gray)
TEXT PRIMARY:          #f4f4f5 (Off-white)
TEXT SECONDARY:        #c4c4cc (Light Gray)
TEXT MUTED:            #a1a1aa (Medium Gray)
PRIMARY COLOR:         #7c3aed (Purple)
SECONDARY COLOR:       #a78bfa (Light Purple)
```

### PREPORUKA 7.1: Pojačani Kontrast za Accessibility

**CSS:**
```css
/* WCAG AA Standard - Minimal 4.5:1 ratio za normal text */

/* Sve interaktivne elemente trebaju biti jasne */
a,
button,
[role="button"] {
    min-height: 44px;
    min-width: 44px;
}

/* Focus outline za accessibility */
*:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

/* Disabled state */
button:disabled,
a:disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

/* Loading state s vidljivim feedback-om */
.loading {
    pointer-events: none;
    opacity: 0.6;
}

/* High contrast mode */
@media (prefers-contrast: more) {
    .custom-card,
    .details-panel,
    .stat-card {
        border-width: 2px;
    }

    .eyebrow {
        font-weight: 700;
        letter-spacing: 3px;
    }

    a {
        text-decoration: underline;
    }
}

/* Reduced motion preference */
@media (prefers-reduced-motion: reduce) {
    * {
        animation-duration: 0.01ms !important;
        animation-iteration-count: 1 !important;
        transition-duration: 0.01ms !important;
    }
}
```

---

### PREPORUKA 7.2: Status Boje - Expanded Palette

```css
/* Success state */
.status-success,
.tag.success {
    background: rgba(16, 185, 129, 0.15);
    border-color: #10b981;
    color: #10b981;
}

/* Warning state */
.status-warning,
.tag.warning {
    background: rgba(245, 158, 11, 0.15);
    border-color: #f59e0b;
    color: #fbbf24;
}

/* Error state */
.status-error,
.tag.error {
    background: rgba(239, 68, 68, 0.15);
    border-color: #ef4444;
    color: #fca5a5;
}

/* Info state */
.status-info,
.tag.info {
    background: rgba(59, 130, 246, 0.15);
    border-color: #3b82f6;
    color: #93c5fd;
}

/* Pending/Loading state */
.status-pending,
.tag.pending {
    background: rgba(107, 114, 128, 0.15);
    border-color: #6b7280;
    color: #d1d5db;
}
```

---

## 📋 AKCIJSKI PLAN

### FAZA 1: OSNOVE (Prioritet 1 - Odmah)
**Trajanje:** 1-2 sata  
**Zadaci:**
- [ ] CSS poboljšanja za breadcrumbs (hover, focus, styling)
- [ ] Dodati focus-visible state-ove na sve linkove i gumbe
- [ ] Poboljšati card hover efekte (border, shadow, transform)
- [ ] Dodati left border na details panele
- [ ] Testirati kontrast s WCAG checker alatom

**Datoteke za promjenu:**
- `wwwroot/css/site.css` - Glavne CSS promjene

---

### FAZA 2: NAVIGACIJA (Prioritet 2 - Sljedeći sprint)
**Trajanje:** 2-3 sata  
**Zadaci:**
- [ ] Reorganizirati main-nav u tri grupe
- [ ] Dodati ikonice u navigaciju
- [ ] Dodati active link state
- [ ] Testirati responsivnost na 640px, 768px

**Datoteke za promjenu:**
- `Views/Shared/_Layout.cshtml` - HTML struktura
- `wwwroot/css/site.css` - Nav CSS

---

### FAZA 3: HOME PAGE ANIMACIJE (Prioritet 3 - Fleksibilno)
**Trajanje:** 2-3 sata  
**Zadaci:**
- [ ] Dodati float animacije u hero sekciju
- [ ] Poboljšati stat cards s ikonama i bounce animacijom
- [ ] Dodati shine efekt na kartice
- [ ] Testirati animacije na 60fps

**Datoteke za promjenu:**
- `Views/Home/Index.cshtml` - HTML (ikonice)
- `wwwroot/css/site.css` - Animacije CSS

---

### FAZA 4: RESPONSIVE & TESTING (Prioritet 2)
**Trajanje:** 1-2 sata  
**Zadaci:**
- [ ] Provjera responsive designa na svim break points
- [ ] Mobile navigacija optimizacija
- [ ] Card layout na mobilnom
- [ ] Browser compatibility provjera (Chrome, Firefox, Edge)

---

## ✅ QUALITY CHECKLIST

Po završetku implementacije, provjeri:

### Vizualna Kvaliteta:
- [ ] Kontrast teksta (sve tekstualne elemente s WCAG checker alatom)
- [ ] Kartice se jasno razlikuju pri hover-u
- [ ] Breadcrumbs su jasne i navigabilne
- [ ] Boje su konzistentne kroz aplikaciju
- [ ] Status badgeovi jasno čitljivi

### Interaktivnost:
- [ ] Svi linkovi imaju hover efekt
- [ ] Svi buttonii imaju active/focus state
- [ ] Transicije su glatke (bez stuttering)
- [ ] Focus outline vidljiv bez scroll-a
- [ ] Bez broken linkova

### Responsive Design:
- [ ] 320px (mobile) - sve je vidljivo i čitljivo
- [ ] 640px (small tablet) - optimalno
- [ ] 768px (tablet) - sve komponente pristupačne
- [ ] 1024px (desktop) - idealan prikaz
- [ ] 1440px (large desktop) - bez previše wide layout-a

### Accessibility:
- [ ] Tab order logičan
- [ ] Sve ikonice imaju alt text ili aria-label
- [ ] Color not only differentiator (shape, text, pattern)
- [ ] Reduced motion preference poštovan
- [ ] High contrast mode poštovan

### Provjera Funkcionalnosti:
- [ ] Sve navigacijske linkove rade
- [ ] Details linkovi funkcioniraju
- [ ] Back linkovi funkcioniraju
- [ ] Nema console error-a
- [ ] Bez broken CSS ili JavaScript-a

---

## 📱 RESPONSIVE BREAKPOINTS

Koristi sljedeće breakpoints:

```css
/* Mobile First */
$mobile: 320px;       /* Min width za sve mobile device */
$small-mobile: 480px; /* Veliki mobili */
$tablet: 768px;       /* iPad mini i veće tablet */
$desktop: 1024px;     /* Desktop počinje */
$large: 1440px;       /* Large desktop */

/* Media queries */
@media (min-width: 768px) { }   /* Tablet i gore */
@media (min-width: 1024px) { }  /* Desktop i gore */
@media (max-width: 767px) { }   /* Tablet i manje */
@media (max-width: 640px) { }   /* Mobile */
```

---

## 📚 RECURSOS I REFERENCE

### WCAG Accessibility:
- https://www.w3.org/WAI/WCAG21/quickref/
- Kontrast ratio checker: https://contrastchecker.com/

### CSS Reference:
- MDN Web Docs: https://developer.mozilla.org/en-US/docs/Web/CSS
- Can I Use: https://caniuse.com/

### Design Guidelines:
- Material Design: https://material.io/
- Tailwind CSS: https://tailwindcss.com/docs

---

## 🎬 ZAKLJUČAK

Ova analiza pruža **konkretne, implementabilne preporuke** za poboljšanje UX/UI aplikacije. Sve preporuke su:

✅ **Fokusirane na UX/UI** - Bez promjena poslovne logike  
✅ **Accessibility-первые** - WCAG AA compliant  
✅ **CSS-based** - Bez dodavanja novih dependency-ja  
✅ **Responsive** - Raduje na svim device-ima  
✅ **Performance-conscious** - Bez nepotrebnih animacija  

Primjena preporuka će rezultirati s:
- Boljom vizualnom hijerarhijom
- Jasnijom navigacijom
- Boljom user feedback-om
- Poboljšanom accessibility-om
- Profesionalnijim izgledom aplikacije

**Sljedeći korak:** Započeti s FAZOM 1 - osnovna CSS poboljšanja.

---

**Report verzija:** 1.0  
**Datum:** 2026-05-05  
**Status:** ✅ Gotovo
