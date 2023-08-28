import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'initials'
})
export class InitialsPipe implements PipeTransform {

	transform(words: string) {
		if (!words) return words;

		var first_letter = function (x) { if (x) { return x[0]; } else { return ''; } };

		return words.split(' ').map(first_letter).join('').toUpperCase();
	}

}