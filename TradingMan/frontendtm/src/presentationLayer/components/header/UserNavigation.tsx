import React, {Fragment} from 'react';
import { Menu, Transition } from '@headlessui/react';
import {Link} from "react-router-dom";

const userNavigation = [
    { id: 0,  name: 'Log out', href: '/' },
]

export interface IUserNavigationProps {
    Logout: () => void,
    user: {email: string, token: string},
}

function UserNavigation (props: IUserNavigationProps) {
    return (
        <Menu as="div" className="ml-3 relative">
            <div>
                <Menu.Button className="max-w-xs px-3 py-2 bg-gray-800 rounded-full flex items-center text-sm focus:outline-none hover:bg-gray-700 hover:text-white active:bg-gray-900 active:text-white">
                    <span className= "text-gray-300 font-medium" >{props.user.email}</span>
                </Menu.Button>
            </div>
            <Transition
                as={Fragment}
                enter="transition ease-out duration-100"
                enterFrom="transform opacity-0 scale-95"
                enterTo="transform opacity-100 scale-100"
                leave="transition ease-in duration-75"
                leaveFrom="transform opacity-100 scale-100"
                leaveTo="transform opacity-0 scale-95"
            >
                <Menu.Items className="z-50 origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none">
                    {userNavigation.map((item) => (
                        <Menu.Item key={item.name}>
                            {({ active }) => (
                                <Link
                                    onClick={item.id === 0 ? props.Logout : undefined}
                                    to={item.href}
                                    className={`block px-4 py-2 text-sm 
											${active
                                        ? "bg-gray-700 text-white"
                                        : "text-gray-800 hover:bg-gray-700 hover:text-white"
                                    }`}
                                >
                                    {item.name}
                                </Link>
                            )}
                        </Menu.Item>
                    ))}
                </Menu.Items>
            </Transition>
        </Menu>
    );
}

export default UserNavigation;