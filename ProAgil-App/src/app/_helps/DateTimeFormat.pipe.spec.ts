/* tslint:disable:no-unused-variable */

import { TestBed, async } from '@angular/core/testing';
import { DateTimeFormatPipe } from './DateTimeFormat.pipe';
import { Constants } from '../util/Constants';

describe('Pipe: DateTimeFormate', () => {
  it('create an instance', () => {
    let pipe = new DateTimeFormatPipe(Constants.DATE_TIME_FMT);
    expect(pipe).toBeTruthy();
  });
});
