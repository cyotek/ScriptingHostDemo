for(var i = 0; i < picture.Length; i++)
{
  let current = picture.getPixel(i);
  let grayscale = toGrayScale(current);

  picture.setPixel(i, grayscale);
}

function toGrayScale(c)
{
  let red = c.R;
  let green = c.G;
  let blue = c.B;
  let gray = red * 0.3 + green * 0.59 + blue * 0.11;

  return color.FromArgb(gray, gray, gray);
}
