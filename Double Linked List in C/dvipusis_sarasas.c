/*
	Autorius: Lukas Valatka
	Užduotis 4.12
	dvipusis_sarasas.c
	Aprašomas duomenų tipas dvipusis sąrašas bei funkcijos, skirtos darbui su juo
  Programavimo kalba: C
  Sukurta: 2015-11-15
  Paskutinė modifikacija: 2015-12-04
*/

#include <stdlib.h>
#include <stdio.h>

// Dvipusio sąrašo elementas turi duomenų kintamąjį, nuorodą į praeitą bei sekantį elementą
struct dvipus_elem {
	int duom;
	struct dvipus_elem *praeitas;
	struct dvipus_elem *sekantis;
};

// Funkcija sukuria dvipusį sąrašą iš failo. Grąžina sąrašo pradžios elementą
// Duomenų failo struktūra:
// 5 [elementų skaičius]
// el1 el2 el3 el4 el5 [elementų reikšmės iš eilės]
struct dvipus_elem *dvipus_saraso_sukurimas(char *failo_pavadinimas) {
	int elem_sk;
	int elem_reiksme;
	int i;
	int *elem_sk_rodykle   = &elem_sk;
	int *elem_reiksmes_ptr = &elem_reiksme;
	struct dvipus_elem *pradzia   = NULL;
	struct dvipus_elem *elementas = NULL;
	struct dvipus_elem *praeitas  = NULL;
	FILE* duom_failas;
	
 	duom_failas = fopen(failo_pavadinimas, "r");

	if (!duom_failas) {
    printf("Klaida perskaitant duomenų failą. \n");
  } else {
	  fscanf(duom_failas, "%d", elem_sk_rodykle);

	  // Perskaitome pradžios elementą atskirai ir išsaugome (užtenka tik jo vėliau operuoti sąrašu)
    printf("------------------------------\n");
	  printf("%s", "Pradedame sąrašo kūrimą... \n");
    
    // Iteruoja nurodytą skaičių elementų tekstiniame faile
    for (i = 0; i < elem_sk; i++) {
		  fscanf(duom_failas, "%d", elem_reiksmes_ptr);
	
		  // Išskiriame atminties elementui, nukreipiame jį ten (malloc grąžina adresą)
		  elementas = (struct dvipus_elem *) malloc( sizeof (struct dvipus_elem) );
    
      // Apdorojame elementą
		  elementas -> praeitas = praeitas; // sulinkinam'e su praeitu elementu
      elementas -> sekantis = NULL; // kol kas tai "šviežiausias" elementas, vadinasi prieky nieko nėra
      elementas -> duom = elem_reiksme;
		
      // Jeigu tai pirmasis elementas, tegu rodyklė "pradžia" rodo į jį
      // Taip pat jei pirmasis, nereikia sulinkint'i praeito su einamu (išvengiam segmentavimo klaidos)
   		if (i == 0) {
			  pradzia = elementas;
		  } else {
		    praeitas -> sekantis = elementas; // praeitas elemento kintamasis sekantis rodo į einamą elementą      
      }
      
      // Galiausiai, naujai iteracijai užfiksuojame elementą "praeitas", kuriuo tampa einamasis elementas
	    praeitas = elementas;
	  }
  }

  // Gražiname rodyklę į pradžios elementą, kuris mus ir domina (tik jo užtenka sėkmingai iteruoti visą sąrašą toliau)
	return pradzia;
}


// Procedūra įterpia elementą prieš nurodytą indeksu dvipusio sąrašo elementą
// Indeksas - naturalusis skaicius (>0)
void iterp_pries_saraso_elem(struct dvipus_elem **rodykl_i_pradzios_rodykl, int duom, int indeksas) {
  int i = 1;
	struct dvipus_elem *pradzia = *rodykl_i_pradzios_rodykl;
  struct dvipus_elem *einamas = NULL;
  struct dvipus_elem *elem    = NULL;
  
  // Esmė - iteruojame dvipusį sąrašą, kol priename elementą su nurodytu indeksu
  // Sukuriame elementą su duom, sekantį jam priskiriame einamą, einamo praeito sekantį - naują sukurtą.
  // Einamo praeitas rodo į sukurtą, o sukurto praeitas - į einamo praeitą.

  // Jeigu neigiamas indeksas, įspėjame vartotoją
  if (indeksas <= 0) {
    printf("Indeksas tik >0 \n");
  } else {
    
    // Sukuriame būsimą elementą
    elem = (struct dvipus_elem *) malloc( sizeof (struct dvipus_elem) );

    do {
      if (i == 1) {
        einamas = pradzia;
        
      } else {
        einamas = einamas->sekantis;     
      } 
      i++;
    } while (einamas != NULL && i <= indeksas);

    if (einamas == NULL) {
      printf("Elemento su tokiu indeksu sąraše nėra \n");
    } else {
      elem -> duom = duom;
      elem -> sekantis = einamas; // sukurtam sekantis - su indeksu nurodytu

      if (einamas -> praeitas) {
        elem -> praeitas = einamas -> praeitas; // sukurtam praeitas - indeksinio praeitas
        einamas -> praeitas -> sekantis = elem; // indeksinio praeito sekantis rodo į sukurtą
      }

      einamas -> praeitas = elem; // einamo praeitas tampa sukurtu elementu

      // Jeigu indeksas = 1, vadinasi įterpiame prieš pradžios elementą
      // (atnaujinam pradžią)
      if (indeksas == 1) {
        *rodykl_i_pradzios_rodykl = elem;
      }

      printf("Įterpėm %d prieš %d \n", duom, indeksas);
    }
  }
}

// Testinė funkcija, skirta atspausdinti sąrašo elementus stulpeliu
void spausdink_sarasa(struct dvipus_elem *pradzia) {
  struct dvipus_elem *elem = pradzia;

  do {
    printf("%d\n", elem->duom);
    elem = elem -> sekantis;
  } while (elem != NULL);
}

void istrinam_sarasa(struct dvipus_elem *pradzia) {
  struct dvipus_elem *elem = pradzia;
  
  do {
    free(elem);
    elem = elem -> sekantis;
  } while (elem != NULL);

}
