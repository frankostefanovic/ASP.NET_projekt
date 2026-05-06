# 🚀 UX/UI BRZI POČETNI VODIČI

**Datum:** 2026-05-05  
**Projekt:** Lab2.RezervacijeProstora  
**Fokus:** Top 5 CSS promjena koje se mogu učiniti u 30 minuta

---

## ⚡ MINUTA 1-5: Focus States (Accessibility)

Dodaj u `wwwroot/css/site.css` nakon zadnje regel-e:

```css
/* ACCESSIBILITY IMPROVEMENTS */

/* Focus outline za sve linkove i gumbe */
*:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}

/* Minimum touch/click target */
a, button, [role="button"] {
    min-height: 44px;
    min-width: 44px;
}

/* Disabled state */
button:disabled,
a[disabled] {
    opacity: 0.5;
    cursor: not-allowed;
}
```

**Rezultat:** ✅ Aplikacija je dostupnija za keyboard navigaciju

---

## ⚡ MINUTA 5-10: Card Hover Effects

Zamijeni `.custom-card` CSS s ovim:

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
    min-height: 280px;
}

.custom-card:hover {
    border-color: #7c3aed;
    background: linear-gradient(135deg, #1a1e2e 0%, rgba(124, 58, 237, 0.15) 100%);
    transform: translateY(-6px);
    box-shadow: 0 16px 32px rgba(124, 58, 237, 0.15);
}

.custom-card h3 {
    color: #f4f4f5;
    font-size: 18px;
    margin: 0 0 8px 0;
    transition: color 0.3s ease;
}

.custom-card:hover h3 {
    color: #a78bfa;
}
```

**Rezultat:** ✅ Kartice su dinamičnije i responzivnije

---

## ⚡ MINUTA 10-15: Details Panel Accents

Zamijeni `.details-panel` CSS s ovim:

```css
.details-panel {
    background: linear-gradient(135deg, #171a23 0%, #1a1e2e 100%);
    border: 1px solid #2b3040;
    border-radius: 16px;
    padding: 28px;
    position: relative;
    overflow: hidden;
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
}

.details-panel h2 {
    color: #f4f4f5;
    font-size: 18px;
    margin: 0 0 16px 0;
    padding-left: 12px;
    border-bottom: 1px solid #2b3040;
    padding-bottom: 12px;
}

.details-panel p,
.details-panel li {
    padding-left: 12px;
}
```

**Rezultat:** ✅ Details paneli imaju bolju vizualnu hijerarhiju

---

## ⚡ MINUTA 15-20: Breadcrumbs Styling

Zamijeni `.breadcrumbs` CSS s ovim:

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
    flex-wrap: wrap;
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

.breadcrumbs span {
    color: #2b3040;
}

.breadcrumbs span:last-child {
    color: #f4f4f5;
    font-weight: 600;
}
```

**Rezultat:** ✅ Breadcrumbs su interaktivnije i jasnije

---

## ⚡ MINUTA 20-30: Button States

Zamijeni `.primary-button` i `.secondary-button` CSS s ovim:

```css
.primary-button,
.secondary-button {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    padding: 14px 28px;
    border-radius: 12px;
    text-decoration: none;
    font-weight: 600;
    font-size: 15px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    border: 2px solid transparent;
    cursor: pointer;
}

.primary-button {
    background: #7c3aed;
    color: white;
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

.details-link {
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
    margin-top: 16px;
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
}

.back-link:hover {
    background: #2b3040;
    border-color: #7c3aed;
    color: #a78bfa;
    transform: translateX(-4px);
}

.back-link:focus-visible {
    outline: 2px solid #a78bfa;
    outline-offset: 2px;
}
```

**Rezultat:** ✅ Svi gumbi i linkovi su konzistentni i responzivni

---

## ✅ REZULTAT NAKON 30 MINUTA:

Aplikacija će imati:
- ✅ Bolju accessibility (focus states)
- ✅ Dinamičnije kartice
- ✅ Bolji visual hierarchy (details paneli s left border)
- ✅ Interaktivnije breadcrumbs
- ✅ Konzistentne button state-ove
- ✅ Sve bez promjene HTML-a ili funkcionalnosti

---

## 🔧 KAKO IMPLEMENTIRATI:

1. **Otvori:** `wwwroot/css/site.css`
2. **Kopiraj** CSS primjere gore
3. **Zamijeni** postojeće CSS règule s novim
4. **Spremi** datoteku (Ctrl+S)
5. **Osvježi** browser (F5 ili Ctrl+Shift+R za hard refresh)
6. **Testiraj** na različitim stranicama

---

## 🧪 TEST CHECKLIST:

Nakon implementacije, provjeri:

- [ ] Hover efekti rade na kartice
- [ ] Focus outline vidljiv pri tab navigaciji
- [ ] Breadcrumbs se osvjetljuju pri hover-u
- [ ] Gumbi imaju smooth transition
- [ ] Details paneli imaju left purple line
- [ ] Nema console error-a
- [ ] Bez promjena u funkcionalnosti
- [ ] Responsive dizajn je očuvan

---

## 📊 PROVJERA PROMJENA:

**Prije:**
- Kartice bez hover efekta
- Nema focus outline-a
- Svi paneli izgledaju isto
- Gumbi bez transitions

**Nakon:**
- Kartice se diže na hover s shadow-om
- Focus outline vidljiv pri tab-u
- Paneli imaju purple left border
- Gumbi se glatko animiraju

---

**Vrijeme potrebno:** ⏱️ ~30 minuta  
**Kompleksnost:** ⭐ Jednostavno (samo CSS)  
**Risk:** 🟢 Nula (samo stilovi, bez logike)  
**Rezultat:** 🎨 Veće poboljšanje vizualne kvalitete
