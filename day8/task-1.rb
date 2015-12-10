#!/usr/bin/env ruby

$lines = Array.new
if not ARGV.empty?
	$fileName = ARGV[0]
	File.open($fileName, "r") do |f|
		f.each_line do |line|
			$lines.push(line.chomp)
		end
	end
else
	ARGF.each_line do |line|
		$lines.push(line.chomp)
	end
end

$charTotal = 0
$stringTotal = 0

$lines.each do |line|
	$charTotal = $charTotal + eval(line).length 
	$stringTotal = $stringTotal + line.length
end

printf "Byte-Character difference: (%d-%d) = %d", $stringTotal, $charTotal, ($stringTotal-$charTotal)
