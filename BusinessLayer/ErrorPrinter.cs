using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class ErrorPrinter
    {
        private Dictionary<int, string> _errorDictionary;
        public ErrorPrinter() {
            this._errorDictionary = new Dictionary<int, string>();
            initializeDictionary();
        }
        
        private void initializeDictionary()
        {
            
            /* --- Error Responses ---
            0 - “No price or commodity type/amount”
                The request is missing the price, commodity or amount property.
            1 - "Bad commodity"
                The request relates to a non existing commodity ID.
            2 - "Bad amount"
                The amount in the request is illegal.
            3 - "No query id"
                The request is missing the id property.
            4- "No commodity"
                The request is missing the commodity property.
            5 - "No auth key"
                The request is missing the auth property.
            6 - "No user or auth token"
                The auth property is missing the user or token property.
            7 - "Verification failure"
                Verification failure.
            8 - "No type key"
                The request is missing the type property.
            9 - "Bad request type"
                The request type is illegal.
            10 - "Id not found"
                The request ID was not found.
            11 - "User does not match"
                The request belongs to a different user.
            12 - "Insufficient funds"
                Not enough funds to complete the request.
            13 - "Insufficient commodity"
                Not enough commodity to complete the request.
                */
            _errorDictionary.Add(0, "No price or commodity type/amount \n\tThe request is missing the price, commodity or amount property.");
            _errorDictionary.Add(1, "Bad commodity \n\tThe request relates to a non existing commodity ID.");
            _errorDictionary.Add(2, "Bad amount \n\tThe amount in the request is illegal.");
            _errorDictionary.Add(3, "No query id \n\tThe request is missing the id property.");
            _errorDictionary.Add(4, "No commodity \n\tThe request is missing the commodity property.");
            _errorDictionary.Add(5, "No auth key \n\tThe request is missing the auth property.");
            _errorDictionary.Add(6, "No user or auth token \n\tThe auth property is missing the user or token property.");
            _errorDictionary.Add(7, "Verification failure \n\tVerification failure.");
            _errorDictionary.Add(8, "No type key\n\tThe request is missing the type property.");
            _errorDictionary.Add(9, "Bad request type\n\tThe request type is illegal.");
            _errorDictionary.Add(10, "Id not found\n\tThe request ID was not found.");
            _errorDictionary.Add(11, "User does not match\n\tThe request belongs to a different user.");
            _errorDictionary.Add(12, "Insufficient funds\n\tNot enough funds to complete the request.");
            _errorDictionary.Add(13, "Insufficient commodity\n\tNot enough commodity to complete the request.");

        }
        public void printError(int key)
        {
            string error = _errorDictionary[key];
            Console.WriteLine(error);
        }
    }
}
