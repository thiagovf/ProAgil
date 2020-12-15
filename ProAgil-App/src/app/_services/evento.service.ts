import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root',
})
export class EventoService {
  tokenHeader: HttpHeaders;
  baseURL = 'http://localhost:5000/api/evento';

  constructor(private http: HttpClient) {
    this.tokenHeader = new HttpHeaders({Authorization: `Bearer ${localStorage.getItem('token')}`});
  }

  getAllEventos(): Observable<Evento[]> {
    
    return this.http.get<Evento[]>(this.baseURL, {headers: this.tokenHeader});
  }

  getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
  }

  getEventoById(eventoId: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${eventoId}`);
  }

  post(evento: Evento): any {
    return this.http.post(this.baseURL, evento, {headers: this.tokenHeader});
  }

  postUpload(file: File, name: string): any {
    const fileToUpload = file[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, name);

    return this.http.post(`${this.baseURL}/upload`, formData, {headers: this.tokenHeader});
  }

  put(evento: Evento): any {
    return this.http.put(`${this.baseURL}/${evento.id}`, evento);
  }

  delete(eventoId: number): any {
    return this.http.delete(`${this.baseURL}/${eventoId}`);
  }
}
