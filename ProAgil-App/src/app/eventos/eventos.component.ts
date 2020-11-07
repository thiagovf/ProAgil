import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { templateJitUrl } from '@angular/compiler';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  bodyDeletarEvento: string;
  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  imagemLargura = 50;
  imagemMargem = 2;
  modoSalvar: string;
  mostrarImagem = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;

  _filtroLista: string;

  constructor(
    private eventoService: EventoService,
    private fb: FormBuilder,
    private localeService: BsLocaleService
  ) {
    this.localeService.use('pt-br');
   }

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this._filtroLista ? this.filtrarEventos(this._filtroLista) : this.eventos;
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  editar(template: any, evento: Evento): void {
    this.openModal(template);
    this.evento = evento;
    this.modoSalvar = 'put';
    this.registerForm.patchValue(evento);
  }

  salvar(template: any): void {
    this.openModal(template);
    this.modoSalvar = 'post';
  }

  excluirEvento(evento: Evento, template: any): void {
    template.show();
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}?`;
  }

  confirmarDelete(template: any): void {
    this.eventoService.delete(this.evento.id).subscribe(
      () => {
        template.hide();
        this.getEventos();
      }, error => {
        console.log(error);
      }
    );
  }

  salvarAlteracao(template: any): void {
    if (this.registerForm.valid) {
      if (this.modoSalvar === 'put') {
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
        this.eventoService.put(this.evento).subscribe(() => {
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        });
      } else {
        this.evento = Object.assign({}, this.registerForm.value);
        this.eventoService.post(this.evento).subscribe(
          (novoEvento: Evento) => {
            template.hide();
            this.getEventos();
          },
          error => {
            console.log(error);
          }
        );
      }
    }
  }

  openModal(template: any): void {
    this.registerForm.reset();
    template.show();
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  getEventos(): void {
    this.eventoService.getAllEventos().subscribe(
      (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = eventos;
      }, error => {
        console.log(error);
      }
    );
  }

}
