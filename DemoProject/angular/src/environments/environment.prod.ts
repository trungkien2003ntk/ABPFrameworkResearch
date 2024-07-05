import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Bookstore',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44325/',
    redirectUri: baseUrl,
    clientId: 'Bookstore_App',
    responseType: 'code',
    scope: 'offline_access Bookstore',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44325',
      rootNamespace: 'MyDemo.Bookstore',
    },
  },
} as Environment;
