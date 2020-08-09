import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventos: any = [
    {
      EventoId: 1,
      Tema: 'Batizado',
      Local: 'Fortaleza'
    },
    {
      EventoId: 2,
      Tema: 'Eucaristia',
      Local: 'Fortaleza'
    }
  ]

  constructor() { }

  ngOnInit() {
  }

}
