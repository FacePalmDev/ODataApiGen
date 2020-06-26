import { Configuration, DATE_PARSER, DECIMAL_PARSER } from 'angular-odata';

//#region ODataApi Imports
{% for import in Imports %}import { {{import.Names | join: ", "}} } from '{{import.Path}}';
{% endfor %}//#endregion

export const {{Name}} = {
  name: '{{Package.Name}}',
  serviceRootUrl: '{{Package.ServiceRootUrl}}',
  creation: new Date('{{Package.Creation | date: "o"}}'),
  schemas: [
    {% for schema in Package.Schemas %}{{schema.Name}}{% unless forloop.last %},
    {% endunless %}{% endfor %}
  ],
  parsers: {
    ...DATE_PARSER,
    ...DECIMAL_PARSER
  }
} as Configuration;