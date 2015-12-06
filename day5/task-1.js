'use strict'

if (process.argv.length == 3) {
	var fs = require('fs');
	fs.readFile(process.argv[2], 'utf8', function(err, data) {
		if (err) {
			console.log(err.message);
			return;
		}
		console.log(processData(data));
	});
} else {
	var total = 0;
	var readline = require('readline');
	var reader = readline.createInterface({
		input: process.stdin,
		output: process.stdout,
		terminal: false
	})
	.on('line', function(line) {
		total += processData(line);
	})
	.on('close', function() {
		console.log(total);
	});
}

function processData(string) {
	string = string.trim();
	var count = 0;
	var strings = string.split("\n");
	for (var i = 0; i < strings.length; i++) {
		if (!strings[i]
			|| !strings[i].match(/[aeiou]/i) 
			|| strings[i].match(/[aeiou]/gi).length < 3
			|| !strings[i].match(/(.)(\1)/i)
			|| strings[i].match(/(ab|cd|pq|xy)/i)) {
				continue;
		}
		count++;
	}
	return count;
}

