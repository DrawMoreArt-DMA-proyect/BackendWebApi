using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Validaciones
{
    public class ValidacionPesoImagen : ValidationAttribute
    {
        public ValidacionPesoImagen(int PesoMaximoMegaBytes)
        {
        }
    }
}
