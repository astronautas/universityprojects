/*
	Autorius: Lukas Valatka
	Užduotis 4.12
	Pagrindinis programos failas
  Programavimo kalba: C
  Sukurta: 2015-11-15
  Paskutinė modifikacija: 2015-12-04
*/ 

#define MAX_FAILO_VARDO_DYDIS 255

#include <stdio.h>
#include <stdlib.h>
#include "dvipusis_sarasas.h"

int main() {
  int skaicius;
  int duom;
	struct dvipus_elem *pradzia;
	char *failo_vardas = (char *) malloc(MAX_FAILO_VARDO_DYDIS);

  printf("Įveskite failo pavadinimą, iš kurio generuosime sąrašą: ");
  scanf("%s", failo_vardas);

	// Sukuriame sąrašą iš failo (jis grąžina rodyklę į pradžios elementą)
	pradzia = dvipus_saraso_sukurimas(failo_vardas);

  // Išspausdiname sukurtą sąrašą
  printf("------------------------------\n");
  printf("Sukurėme sąrašą su elementais: \n");
  spausdink_sarasa(pradzia);
  printf("------------------------------\n");
  
  if (pradzia) {
    printf("Įveskite elemento numerį (1, 2, 3 ... < sąrašo ilgis), prieš kurį norėsite įterpti sąraše: ");

    if (scanf("%d", &skaicius)) {
      printf("Įveskite reikšmę elemento (integer), kuris bus įterptas prieš %d elementą: ", skaicius);
     
      if (scanf("%d", &duom)) {
        // Paduodam rodyklės adresą, nes reikia, kad galbūt pakeistume jos reikšmę funkcijos išorėje
        iterp_pries_saraso_elem(&pradzia, duom, skaicius);
      } else {
        printf("Įvesta reikšmė turi būti skaičius.\n");
      }
    } else {
        printf("Įvesta reikšmė turi būti skaičius.\n");
    }
  }
  
  spausdink_sarasa(pradzia);
  
  // Ištrina sąrašą - atlaisviname atmintį
  istrinam_sarasa(pradzia);
  free(failo_vardas);

	return 0;
}
